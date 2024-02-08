using UnityEngine;

namespace gather
{
    public interface ITargetable 
    {
        bool CanTarget(int team);
        public Vector2 CurrentLocation();
    }
}