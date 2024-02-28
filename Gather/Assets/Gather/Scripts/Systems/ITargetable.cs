using UnityEngine;

namespace Gather
{
    public interface ITargetable 
    {
        bool CanBeTargeted(int team);
        public Vector2 GetLocation();
    }
}