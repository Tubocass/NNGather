using System.Collections;
using System.Collections.Generic;
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
        float timer;
        protected bool isMoving;
        // Animator

        public bool IsMoving { get { return isMoving; } }

        protected virtual void Awake()
        {
            myTransform = transform; 
            spriteRenderer = GetComponent<SpriteRenderer>();
            navAgent = GetComponent<PolyNavAgent>();
        }

        private void Update()
        {
            timer += Time.deltaTime;
            if (timer >= 0.125)
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
    }
}
