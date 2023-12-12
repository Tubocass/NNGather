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

        EnemyDetector enemyDetector;
        FarmerDrone drone;
        //Blackboard context = new Blackboard();

        public FarmerFSM_Controller(FarmerDrone farmerDrone, EnemyDetector enemyDetector, Blackboard context)
        {
            this.drone = farmerDrone;
            this.enemyDetector = enemyDetector;
            //this.context = context;

            searchState = new State_Search(drone, context);
            fleeState = new State_Flee(drone, context);
            returnState = new State_Return(drone);
            
            enemyDetector.SetEnemyType(unit => unit.GetType() == typeof(FighterDrone));
            enemyDetector.EnemyDetected += Flee;
            enemyDetector.AllClear += AssessSituation;
        }

        public void Enable(int team)
        {
            enemyDetector.SetTeam(team);
            SearchForFood();
        }

        public void AssessSituation()
        {
            if (drone.IsCarryingFood())
            {
                if (ActiveState.GetStateName() != States.returnToQueen)
                {
                    ActiveState = returnState;
                }else
                {
                    ActiveState.Update();
                }
            }
            else if (ActiveState.GetStateName() != States.search)
            {
                SearchForFood();
            }else
            {
                ActiveState.Update();
            }
        }

        public override void Disable()
        {
            base.Disable();
        }

        void SearchForFood()
        {
            ActiveState = searchState;
        }

        void Flee()
        {
            //HaltNavigation();
            //fleeState.SetEnemiesList(enemyDetector.GetEnemiesList());
            ActiveState = fleeState;
        }
    }
}