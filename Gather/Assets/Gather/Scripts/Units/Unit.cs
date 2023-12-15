using UnityEngine;
using PolyNav;
using Gather.AI;


namespace gather
{
    public abstract class Unit : MonoBehaviour, ITarget
    {
        public TeamConfig teamConfig;
        protected Transform myTransform;
        protected SpriteRenderer spriteRenderer;
        protected PolyNavAgent navAgent;
        protected FSM_Controller fsmController;
        protected EnemyDetector enemyDetector;
        [SerializeField] protected SearchConfig enemySearchConfig;
        [SerializeField] protected UnitType unitType;
        [SerializeField] float updateTime = 0.125f;
        float timer;
        protected bool isMoving;
        // Animator

        public bool IsMoving { get { return isMoving; } }

        protected virtual void Awake()
        {
            myTransform = transform; 
            spriteRenderer = GetComponent<SpriteRenderer>();
            navAgent = GetComponent<PolyNavAgent>();
            enemyDetector = new EnemyDetector(this, enemySearchConfig);
        }

        private void Update()
        {
            timer += Time.deltaTime;
            if (timer >= updateTime)
            {
                timer = 0;
                fsmController.Update();
            }
        }

        public Vector2 Location()
        {
            return myTransform.position;
        }

        public virtual void SetTeam(TeamConfig config)
        {
            this.teamConfig = config;
            enemyDetector.SetTeam(GetTeam());
            SetTeamColor();
        }

        public void SetTeamColor()
        {
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
            teamConfig.SetUnitCount(unitType, -1);
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

        public void SetDestination(Vector2 location, System.Action<bool> callback)
        {
            navAgent.SetDestination(location, callback);
        }

        void DestinationReached(bool reached)
        {
            isMoving = !reached;
        }

        public bool GetEnemyDetected()
        {
            return enemyDetector.Detect();
        }

        public UnitType GetUnitType()
        {
            return unitType;
        }
    }
}
