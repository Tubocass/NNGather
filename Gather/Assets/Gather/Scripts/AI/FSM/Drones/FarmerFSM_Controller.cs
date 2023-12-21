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

        public FarmerFSM_Controller(FarmerDrone farmerDrone, Blackboard context)
        {
            searchState = new State_Search(farmerDrone, context);
            fleeState = new State_Flee(farmerDrone, context);
            returnState = new State_Return(farmerDrone, context);
            engageState = new State_Engage(farmerDrone, context);
            initialState = searchState;

            searchState.transistions.Add(new ToStateFlee(farmerDrone, fleeState));
            searchState.transistions.Add(new ToStateEngage(farmerDrone, engageState));
            searchState.transistions.Add(new ToStateReturn(farmerDrone, returnState));

            engageState.transistions.Add(new ToStateFlee(farmerDrone, fleeState));
            engageState.transistions.Add(new ToStateReturn(farmerDrone, returnState));
            engageState.transistions.Add(new ToStateSearch(farmerDrone, searchState));


            fleeState.transistions.Add(new ToStateSearch(farmerDrone, searchState));
            fleeState.transistions.Add(new ToStateReturn(farmerDrone, returnState));

            returnState.transistions.Add(new ToStateFlee(farmerDrone, fleeState));
            returnState.transistions.Add(new ToStateSearch(farmerDrone, searchState));
        }
    }
}