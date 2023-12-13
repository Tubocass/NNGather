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

        public QueenFSM_Controller (Queen queen, Blackboard context)
        {
            moveState = new State_Move(queen, context);
            feedState = new State_Feed(queen, context);
            spawnState = new State_Spawn(queen, context);
            initialState = spawnState;

            moveState.transistions.Add(new ToStateFeed(queen, feedState));
            moveState.transistions.Add(new ToStateSpawn(queen, spawnState));

            feedState.transistions.Add(new ToStateSpawn(queen, spawnState));
            feedState.transistions.Add(new ToStateMove(queen, moveState));

            spawnState.transistions.Add(new ToStateFeed(queen, feedState));
            spawnState.transistions.Add(new ToStateMove(queen, moveState));
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
