﻿using gather;
using Gather.AI.FSM.States;
using Gather.AI.FSM.Transitions;

namespace Gather.AI.FSM.Controllers
{
    public class QueenFSM_Controller : FSM_Controller
    {
        Queen queen;
        EnemyDetector enemyDetector;

        protected override void Init()
        {
            queen = GetComponent<Queen>();
            enemyDetector = GetComponent<EnemyDetector>();

            State_Move moveState = new State_Move(queen);
            State_Feed feedState = new State_Feed(queen);
            State_Spawn spawnState = new State_Spawn(queen);
            State_Emergency emergencyState = new State_Emergency(queen);
            initialState = spawnState;

            ToStateFeed toFeed = new ToStateFeed(queen, feedState);
            ToStateSpawn toSpawn = new ToStateSpawn(queen, spawnState);
            ToStateFlee toFlee = new ToStateFlee(queen, emergencyState);
            ToStateMove toMove = new ToStateMove(queen, moveState);

            moveState.AddTransitions(toFlee, toFeed, toSpawn);
            feedState.AddTransitions(toFlee, toMove, toSpawn);
            spawnState.AddTransitions(toFlee, toFeed, toMove);
            emergencyState.AddTransitions(toFeed, toSpawn);
        }

        public override void Tick()
        {
            enemyDetector.Detect();
            base.Tick();
        }
    }
}
