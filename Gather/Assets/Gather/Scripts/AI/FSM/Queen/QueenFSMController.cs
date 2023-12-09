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
        Queen queen;
        Counter foodCounter;
        TeamConfig teamConfig;
        Blackboard context;
        
        private int hunger = 1;
        int targetFoodCount;


        public QueenFSMController (Queen queen, Blackboard context)
        {
            this.queen = queen;
            this.context = context;
            moveState = new State_Move(queen, context);
            feedState = new State_Feed(queen, context);
            spawnState = new State_Spawn(queen, context);

            queen.QueenMove += Feed;
            feedState.Finished += Spawn;
            spawnState.Finished += Move;
        }

        public void Enable(int team)
        {
            foodCounter = context.GetValue<Counter>(Configs.FoodCounter);
            teamConfig = context.GetValue<TeamConfig>(Configs.TeamConfig);

            Spawn();
            AssessSituation();
        }

        public void AssessSituation()
        {
            /* 
             * called on Gather
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