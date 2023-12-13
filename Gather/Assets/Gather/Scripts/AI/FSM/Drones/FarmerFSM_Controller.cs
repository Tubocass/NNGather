using System.Collections;
using UnityEngine;
using gather;

namespace Gather.AI
{
    public class FarmerFSM_Controller : FSM_Controller, AIController_Interface
    {
        State_Search searchState;
        State_Flee fleeState;
        State_Return returnState;

        public FarmerFSM_Controller(FarmerDrone farmerDrone, Blackboard context)
        {
            searchState = new State_Search(farmerDrone, context);
            fleeState = new State_Flee(farmerDrone, context);
            returnState = new State_Return(farmerDrone, context);
            initialState = searchState;

            searchState.transistions.Add(new ToStateFlee(farmerDrone, fleeState));
            searchState.transistions.Add(new ToStateReturn(farmerDrone, returnState));

            fleeState.transistions.Add(new ToStateSearch(farmerDrone, searchState));
            fleeState.transistions.Add(new ToStateReturn(farmerDrone, returnState));

            returnState.transistions.Add(new ToStateFlee(farmerDrone, fleeState));
            returnState.transistions.Add(new ToStateSearch(farmerDrone, searchState));
        }
    }
}