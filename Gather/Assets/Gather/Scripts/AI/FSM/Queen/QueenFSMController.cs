using System.Collections;
using UnityEngine;
using gather;

namespace Gather.AI
{
    public class QueenFSMController : FSMController, AIController_Interface
    {
        //Blackboard context = new Blackboard();

        State_Move moveState;
        State_Feed feedState;
        // State_Gestate
        State_Spawn spawnState;
        /*  Feed until drones can be created
         *  focus on gestating (1-2 seconds)
         *  pop out drones
         *  move to new location
         *  repeat
        */

        public QueenFSMController (Queen queen, Blackboard context)
        {
            moveState = new State_Move(queen, context);
            feedState = new State_Feed(queen, context);
            spawnState = new State_Spawn(queen, context);

            feedState.Finished += Spawn;
            moveState.QueenMove += Feed;
            spawnState.Finished += Move;
        }

        public void Enable(int team)
        {
            Spawn();
        }

        public void AssessSituation()
        {
            /*  if(food < minimum)
             *      Feed
             *  if(drones < desired)
             *      Spawn
            */
            BehaviorState.AssesSituation();
        }

        public void Move()
        {
            BehaviorState = moveState;
        }

        public void Feed()
        {
            BehaviorState = feedState;
        }
        public void Spawn()
        {
            BehaviorState = spawnState;
        }
    }
}