using UnityEngine;

namespace gather
{
    public class FighterDrone : Drone
    {
        public override void SetQueen(Queen queenie)
        {
            base.SetQueen(queenie);
            queenie.redFlag += SetDestination;
        }
        public override Vector2 AnchorPoint()
        {
            return myQueen.FightAnchor.position;
        }
    }
}