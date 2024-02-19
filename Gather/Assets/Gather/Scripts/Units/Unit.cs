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
    [RequireComponent(typeof(Health))]

    public abstract class Unit : MonoBehaviour, ITargetable
    {
        [SerializeField] protected UnitType unitType;
        protected Blackboard context = new Blackboard();
        protected EnemyDetector enemyDetector;
        protected PolyNavAgent navAgent;
        protected SpriteRenderer spriteRenderer;
        protected TeamConfig teamConfig;
        protected Transform myTransform;
        protected Health health;
        protected bool isMoving;
        private bool hasTarget;
        // Animator

        public bool IsMoving { get => isMoving; }
        public UnitType UnitType => unitType;
        public bool HasTarget => hasTarget;
        public Blackboard Blackboard => context;
        public Health Health => health;
        public TeamConfig TeamConfig => teamConfig;

        protected virtual void Awake()
        {
            myTransform = transform; 
            spriteRenderer = GetComponent<SpriteRenderer>();
            navAgent = GetComponent<PolyNavAgent>();
            enemyDetector = GetComponent<EnemyDetector>();
            health = GetComponent<Health>();
            context.SetValue(Configs.EnemyDetector, enemyDetector);
        }

        public Vector2 GetLocation()
        {
            return myTransform.position;
        }

        public virtual void SetTeam(TeamConfig config)
        {
            this.teamConfig = config;
            teamConfig.UnitManager.UpdateUnitCount(unitType, 1);
            enemyDetector.SetTeam(teamConfig.TeamID);
            spriteRenderer.color = teamConfig.TeamColor;
        }

        public int GetTeamID()
        {
            return teamConfig.TeamID;
        }

        public virtual void Death()
        {
            gameObject.SetActive(false);
        }

        protected virtual void OnDisable()
        {
            teamConfig?.UnitManager.UpdateUnitCount(unitType, -1);
            context.Clear();
        }

        public bool CanBeTargeted(int team)
        {
            return teamConfig.TeamID != team && gameObject.activeSelf;
        }

        public void SetDestination(Vector2 location)
        {
            isMoving = true;
            navAgent.SetDestination(location, (reached) => isMoving = !reached);
        }

        public bool GetEnemyDetected()
        {
            return enemyDetector.DetectedThing;
        }

        public void SetHasTarget(bool value)
        {
            hasTarget = value;
        }
    }
}