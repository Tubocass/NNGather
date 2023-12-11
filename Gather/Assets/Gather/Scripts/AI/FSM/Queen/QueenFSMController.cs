﻿using gather;
using UnityEngine;
using System.Collections.Generic;

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
        QueenStatus queenStatus;
        Queue<Vector2> foodLocations;

        private int hunger = 1;
        int targetFoodCount;

        public QueenFSMController (Queen queen, Blackboard context)
        {
            //this.queen = queen;
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
            foodLocations = context.GetValue<Queue<Vector2>>(Configs.FoodLocations);

            Spawn();
            AssessSituation();
        }

        public void AssessSituation()
        {
            BehaviorState.AssesSituation();
        }

        public void Move()
        {
            BehaviorState = moveState;
        }

        public void Move(Vector2 target)
        {
            moveState.SetTargetDestination(target);
            BehaviorState = moveState;
        }

        public void Feed()
        {
            BehaviorState = feedState;
        }

        public void Spawn()
        {
            SpawnJob job = new SpawnJob(8, 2);
            context.SetValue(Configs.SpawnJob, job);
            BehaviorState = spawnState;
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