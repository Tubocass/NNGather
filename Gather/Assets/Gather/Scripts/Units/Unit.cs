using UnityEngine;
using PolyNav;
using Gather.AI;

namespace Gather
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
        // Animator

        public bool IsMoving { get => isMoving; }
        public UnitType UnitType => unitType;
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
            context.SetValue(Keys.EnemyDetector, enemyDetector);
            context.SetValue(Keys.Unit, this);
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
            return teamConfig.TeamID != team && isActiveAndEnabled;
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

        public virtual void SetHasTarget(bool value)
        {
            context.SetValue(Keys.HasTarget, value);
        }
    }
}