using System.Collections;
using UnityEngine;
using gather;

namespace Gather.AI
{
    public class State_Move : IBehaviorState
    {
        public GameEvent QueenMove;
        Queen queen;
        Blackboard context;

        public State_Move(Queen queen, Blackboard context)
        {
            this.queen = queen;
            this.context = context;
        }

        public void EnterState()
        {
            Queue foods = context.GetValue<Queue>(Configs.FoodLocations);
            Vector2 newPosition = queen.Location();
            int size = foods.Count;
            for (int np = size; np > 0; np--)
            {
                newPosition += (Vector2)foods.Dequeue();
            }
            newPosition /= size;
            queen.SetDestination(newPosition);
            //QueenMove?.Invoke();
        }

        public void AssesSituation()
        {
            throw new System.NotImplementedException();
        }

        public void ExitState()
        {
            //QueenMove?.Invoke();
        }
    }
}