using System.Collections;
using UnityEngine;
using gather;

namespace Gather.AI
{
    public class FighterFSMController : FSMController, AIController_Interface
    {
        //EnemyDetector enemyDetector;
        //FighterDrone drone;
        //Blackboard context = new Blackboard();

        State_Hunt huntState;
        State_Engage engageState;

        public FighterFSMController (FighterDrone drone, Blackboard context)
        {
            huntState = new State_Hunt(drone, context);
            huntState.TargetFound += TargetFound;

            engageState = new State_Engage(drone, context);
            engageState.TargetLost += StartHunt;
            engageState.TargetReached += StartHunt;
        }
        public void Enable(int team)
        {
            //enemyDetector.SetTeam(team);
            StartHunt();
        }

        public void AssessSituation()
        {
            BehaviorState.AssesSituation();
        }

        public override void Disable()
        {
            base.Disable();
        }

        public void TargetFound()
        {
            BehaviorState = engageState;
        }

        public void StartHunt()
        {
            BehaviorState = huntState;
        }
    }
}