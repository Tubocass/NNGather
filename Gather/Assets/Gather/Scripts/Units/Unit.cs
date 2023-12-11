using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PolyNav;


namespace gather
{
    public class Unit : MonoBehaviour, ITarget
    {
        public TeamConfig teamConfig;
        protected Transform myTransform;
        protected SpriteRenderer spriteRenderer;
        protected PolyNavAgent navAgent;
        // Animator

        protected virtual void Awake()
        {
            myTransform = transform; 
            spriteRenderer = GetComponent<SpriteRenderer>();
            navAgent = GetComponent<PolyNavAgent>();
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
            navAgent.SetDestination(location);
        }
    }
}
