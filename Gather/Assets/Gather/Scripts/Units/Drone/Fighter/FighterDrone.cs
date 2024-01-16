using UnityEngine;

namespace gather
{
    public class FighterDrone : Drone
    {
        Anchor fightAnchor;
        public override void SetQueen(Queen queenie)
        {
            base.SetQueen(queenie);
            fightAnchor = queenie.fightAnchor;
            fightAnchor.PlaceAnchor += SetDestination;
        }

        public override void Death()
        {
            if (fightAnchor != null )
            {
                fightAnchor.PlaceAnchor -= SetDestination;
                fightAnchor = null;
            }
            base.Death();
        }
        public override Vector2 AnchorPoint()
        {
            return fightAnchor.GetActive() ? fightAnchor.GetLocation() : myQueen.Location();
        }
    }
}