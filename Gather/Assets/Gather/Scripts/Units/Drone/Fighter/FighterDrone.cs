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
        public override Vector2 AnchorPoint()
        {
            return fightAnchor.GetActive() ? fightAnchor.GetPosition() : Location();
        }
    }
}