using System.Collections;
using UnityEngine;

namespace Gather.AI
{
    public interface FSM_Transistion
    {
        public bool isValid();
        public void OnTransition();
        public FSM_State GetNextState();
    }
}