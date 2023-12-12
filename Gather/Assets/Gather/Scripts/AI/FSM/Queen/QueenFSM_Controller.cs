using gather;
using UnityEngine;
using System.Collections.Generic;

namespace Gather.AI
{
    public class QueenFSM_Controller : FSM_Controller, AIController_Interface
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
        QueenStatus queenStatus;
        Queue<Vector2> foodLocations;

        private int hunger = 1;
        int targetFoodCount;

        public QueenFSM_Controller (Queen queen, Blackboard context)
        {
            //this.queen = queen;
            this.context = context;
            moveState = new State_Move(queen, context);
            feedState = new State_Feed(queen, context);
            spawnState = new State_Spawn(queen, context);

            moveState.QueenMove += Feed;
            feedState.Finished += Spawn;
            spawnState.Finished += Move;
        }

        public void Enable(int team)
        {
            foodCounter = context.GetValue<Counter>(Configs.FoodCounter);
            teamConfig = context.GetValue<TeamConfig>(Configs.TeamConfig);
            foodLocations = context.GetValue<Queue<Vector2>>(Configs.FoodLocations);

            Spawn();
            AssessSituation();
        }

        public void AssessSituation()
        {
            ActiveState.Update();
        }

        public void Move()
        {
            ActiveState = moveState;
        }

        public void Move(Vector2 target)
        {
            moveState.SetTargetDestination(target);
            ActiveState = moveState;
        }

        public void Feed()
        {
            ActiveState = feedState;
        }

        public void Spawn()
        {
            SpawnJob job = new SpawnJob(8, 2);
            context.SetValue(Configs.SpawnJob, job);
            ActiveState = spawnState;
        }
    }
    
    public class QueenStatus
    {
        public enum FoodStatus { None, Low, Medium, Full}
        public enum DroneStatus { None, Low, Medium, Full}

        public FoodStatus foodStatus;
        public DroneStatus farmerStatus;
        public DroneStatus fighterStatus;

        public FoodStatus EvalFood(Counter foodCounter)
        {
            /*
             * account for: 
             * hunger
             * food limit
            */
            return FoodStatus.None;
        }

        public DroneStatus EvalFarmers(TeamConfig team)
        {
            /*
             * account for: 
             * food status
             * farmer cap
            */
            return DroneStatus.None;
        }

        public DroneStatus EvalFighters(TeamConfig team)
        {
            return DroneStatus.None;
        }

        public void Evaluate(TeamConfig team, Counter foodCounter)
        {
            foodStatus = EvalFood(foodCounter);
            farmerStatus = EvalFarmers(team);
            fighterStatus = EvalFighters(team);
        }
    }
}
