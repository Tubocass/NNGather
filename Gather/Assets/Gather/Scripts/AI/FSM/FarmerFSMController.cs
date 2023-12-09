using System.Collections;
using UnityEngine;
using gather;

namespace Gather.AI
{
    public class FarmerFSMController : FSMController, AIController_Interface
    {
        State_Search searchState;
        State_Flee fleeState;
        State_Return returnState;

        EnemyDetector enemyDetector;
        FarmerDrone drone;
        //Blackboard context = new Blackboard();

        public FarmerFSMController(FarmerDrone farmerDrone, EnemyDetector enemyDetector, Blackboard context)
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
            drone.GetMyQueen().QueenMove += returnState.QueenMoved;

            SearchForFood();
        }

        public void AssessSituation()
        {
            if (drone.IsCarryingFood())
            {
                if (BehaviorState.ToString() != States.returnToQueen)
                {
                    BehaviorState = returnState;
                }else
                {
                    BehaviorState.AssesSituation();
                }
            }
            else if (BehaviorState.ToString() != States.search)
            {
                SearchForFood();
            }else
            {
                BehaviorState.AssesSituation();
            }
        }

        public override void Disable()
        {
            base.Disable();
            drone.GetMyQueen().QueenMove -= returnState.QueenMoved;
        }

        void SearchForFood()
        {
            BehaviorState = searchState;
        }

        void Flee()
        {
            //HaltNavigation();
            //fleeState.SetEnemiesList(enemyDetector.GetEnemiesList());
            BehaviorState = fleeState;
        }
    }
}