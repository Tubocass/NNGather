using System.Collections.Generic;
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
            Queue<Vector2> foods = context.GetValue<Queue<Vector2>>(Configs.FoodLocations);
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
        }

        public void ExitState()
        {
            //QueenMove?.Invoke();
        }

        string IBehaviorState.ToString()
        {
            return States.move;
        }
    }
}