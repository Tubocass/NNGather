using System.Collections;
using UnityEngine;
using gather;

namespace Gather.AI
{
    public class FighterFSM_Controller : FSM_Controller, AIController_Interface
    {
        //EnemyDetector enemyDetector;
        //FighterDrone drone;
        //Blackboard context = new Blackboard();

        State_Hunt huntState;
        State_Engage engageState;

        public FighterFSM_Controller (FighterDrone drone, Blackboard context)
        {
            huntState = new State_Hunt(drone, context);
            huntState.TargetFound += TargetFound;

            engageState = new State_Engage(drone, context);
            engageState.TargetLost += StartHunt;
            engageState.TargetReached += StartHunt;

            initialState = huntState;
        }
        public void Enable()
        {
            //enemyDetector.SetTeam(team);
            ActiveState = initialState;
        }

        public void TargetFound()
        {
            ActiveState = engageState;
        }

        public void StartHunt()
        {
            ActiveState = huntState;
        }
    }
}