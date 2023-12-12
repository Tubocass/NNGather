using System.Collections.Generic;
using UnityEngine;
using gather;

namespace Gather.AI
{
    public class State_Move : FSM_State
    {
        public GameEvent QueenMove;
        Queen queen;
        Blackboard context;
        Queue<Vector2> foods;
        Vector2 targetDestination = Vector2.zero;

        public State_Move(Queen queen, Blackboard context)
        {
            this.queen = queen;
            this.context = context;
            foods = context.GetValue<Queue<Vector2>>(Configs.FoodLocations);

        }

        public override void EnterState()
        {
            if(targetDestination != Vector2.zero)
            {
                queen.SetDestination(targetDestination, DestinationReached);
            }else
            {
                MoveToFoodCenter();
            }
        }

        public void SetTargetDestination(Vector2 target)
        {
            targetDestination = target;
        }

        void MoveToFoodCenter()
        {
            targetDestination = queen.Location();
            int size = foods.Count;
            for (int np = size; np > 0; np--)
            {
                targetDestination += foods.Dequeue();
            }
            targetDestination /= size;
            queen.SetDestination(targetDestination, DestinationReached);
        }

        public override void Update()
        {
         
        }

        public override void ExitState()
        {
            targetDestination = Vector2.zero;
        }

        public override string GetStateName()
        {
            return States.move;
        }

        void DestinationReached(bool reached)
        {
            QueenMove?.Invoke();
        }
    }
}