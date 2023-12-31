using System.Collections;
using UnityEngine;

namespace gather
{
    public interface ITarget 
    {
        bool CanTarget(int team);
        public Vector2 Location();
    }
}