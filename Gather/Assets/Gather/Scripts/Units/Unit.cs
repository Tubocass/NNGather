using UnityEngine;
using PolyNav;
using Gather.AI;

namespace gather
{
    public enum UnitType { Fighter, Farmer, Queen, Sarlac }

    [RequireComponent(typeof(EnemyDetector))]
    [RequireComponent(typeof(PolyNavAgent))]
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Rigidbody2D))]

    public abstract class Unit : MonoBehaviour, ITargetable, ITargeter
    {
        [SerializeField] protected UnitType unitType;
        protected Blackboard context = new Blackboard();
        protected EnemyDetector enemyDetector;
        protected FSM_Controller fsmController;
        protected PolyNavAgent navAgent;
        protected SpriteRenderer spriteRenderer;
        protected TeamConfig teamConfig;
        protected Transform myTransform;
        protected bool isMoving;
        private bool isEnemyDetected;
        private bool hasTarget;
        // Animator

        protected virtual void Awake()
        {
            myTransform = transform; 
            spriteRenderer = GetComponent<SpriteRenderer>();
            navAgent = GetComponent<PolyNavAgent>();
            enemyDetector = GetComponent<EnemyDetector>();
            fsmController = GetComponent<FSM_Controller>();
            context.SetValue(Configs.EnemyDetector, enemyDetector);
        }

        public bool IsMoving { get => isMoving; }

        public Vector2 CurrentLocation()
        {
            return myTransform.position;
        }

        public virtual void SetTeam(TeamConfig config)
        {
            this.teamConfig = config;
            teamConfig.UpdateUnitCount(unitType, 1);
            enemyDetector.SetTeam(teamConfig.Team);
            spriteRenderer.color = teamConfig.TeamColor;
        }

        public int GetTeam()
        {
            return teamConfig.Team;
        }

        public virtual void Death()
        {
            gameObject.SetActive(false);
        }

        protected virtual void OnDisable()
        {
            teamConfig?.UpdateUnitCount(unitType, -1);
            context.Clear();
        }

        public bool CanTarget(int team)
        {
            return teamConfig.Team != team && gameObject.activeSelf;
        }

        public void SetDestination(Vector2 location)
        {
            isMoving = true;
            navAgent.SetDestination(location, DestinationReached);
        }

        void DestinationReached(bool reached)
        {
            isMoving = !reached;
        }

        public void DetectEnemeies()
        {
            isEnemyDetected = enemyDetector.Detect();
        }

        public bool GetEnemyDetected()
        {
            return isEnemyDetected;
        }

        public UnitType GetUnitType()
        {
            return unitType;
        }

        public Blackboard GetBlackboard()
        {
            return context;
        }

        public bool HasTarget()
        {
            return hasTarget;
        }

        public void SetHasTarget(bool value)
        {
            hasTarget = value;
        }
    }
}