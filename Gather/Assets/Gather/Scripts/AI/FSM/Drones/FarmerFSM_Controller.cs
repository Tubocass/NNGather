using System.Collections;
using UnityEngine;
using gather;

namespace Gather.AI
{
    public class FarmerFSM_Controller : FSM_Controller
    {
        State_Search searchState;
        State_Flee fleeState;
        State_Return returnState;
        State_Engage engageState;
        FarmerDrone drone;

        protected override void Init()
        {
            drone = GetComponent<FarmerDrone>();
            Blackboard context = drone.GetBlackboard();
            searchState = new State_Search(drone, context);
            fleeState = new State_Flee(drone, context);
            returnState = new State_Return(drone, context);
            engageState = new State_Engage(drone, context);
            initialState = searchState;

            searchState.transistions.Add(new ToStateFlee(drone, fleeState));
            searchState.transistions.Add(new ToStateEngage(drone, engageState));
            searchState.transistions.Add(new ToStateReturn(drone, returnState));

            engageState.transistions.Add(new ToStateFlee(drone, fleeState));
            engageState.transistions.Add(new ToStateReturn(drone, returnState));
            engageState.transistions.Add(new ToStateSearch(drone, searchState));


            fleeState.transistions.Add(new ToStateSearch(drone, searchState));
            fleeState.transistions.Add(new ToStateReturn(drone, returnState));

            returnState.transistions.Add(new ToStateFlee(drone, fleeState));
            returnState.transistions.Add(new ToStateSearch(drone, searchState));
        }

        public override void Tick()
        {
            drone.DetectEnemeies();
            base.Tick();
        }
    }
}