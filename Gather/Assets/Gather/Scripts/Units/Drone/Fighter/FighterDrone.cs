using Gather.AI.FSM.Controllers;
using System.Collections;
using UnityEngine;

namespace gather
{
    [RequireComponent(typeof(FighterFSM_Controller))]
    public class FighterDrone : Drone
    {
        Anchor fightAnchor;
      

        protected override void Awake()
        {
            base.Awake();
        }

        public override void SetQueen(Queen queenie)
        {
            base.SetQueen(queenie);
            fightAnchor = queenie.fightAnchor;
            fightAnchor.PlaceAnchor += SetDestination;
        }

        public override void Death()
        {
            StopAllCoroutines();
            if (fightAnchor != null )
            {
                fightAnchor.PlaceAnchor -= SetDestination;
                fightAnchor = null;
            }
            base.Death();
        }

        public override Vector2 AnchorPoint()
        {
            return fightAnchor.IsActive() ? fightAnchor.GetLocation() : myQueen.GetLocation();
        }
    }
}