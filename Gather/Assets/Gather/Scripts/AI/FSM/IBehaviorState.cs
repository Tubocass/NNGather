using System.Collections;
using UnityEngine;

namespace Gather.AI
{
    public interface IBehaviorState 
    {
        void EnterState();
        void AssesSituation();
        void ExitState();
        string ToString();
    }
}