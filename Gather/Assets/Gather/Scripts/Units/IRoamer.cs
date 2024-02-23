using UnityEngine;

namespace gather
{
    public interface IRoamer 
    {
        public void MoveRandomly(Vector2 center);
        public Vector2 AnchorPoint();
        public void ReturnHome();
    }
}