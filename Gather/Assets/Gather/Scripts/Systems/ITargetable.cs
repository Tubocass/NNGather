using UnityEngine;

namespace gather
{
    public interface ITargetable 
    {
        bool CanBeTargeted(int team);
        public Vector2 GetLocation();
    }
}