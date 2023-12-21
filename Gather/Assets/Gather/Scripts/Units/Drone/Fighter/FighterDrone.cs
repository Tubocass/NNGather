using System.Collections;
using UnityEngine;
using Gather.AI;

namespace gather
{
    public class FighterDrone : Drone
    {
        protected override void Awake()
        {
            base.Awake();

            fsmController = new FighterFSM_Controller(this, context);
        }

        public override void SetQueen(Queen queenie)
        {
            base.SetQueen(queenie);
            queenie.redFlag += SetDestination;
        }
    }
}
