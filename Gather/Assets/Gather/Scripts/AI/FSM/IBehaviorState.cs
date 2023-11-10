using System.Collections;
using UnityEngine;

namespace gather
{
    public interface IBehaviorState 
    {
        void EnterState();
        void AssesSituation();
        void ExitState();
        string ToString();
    }
}