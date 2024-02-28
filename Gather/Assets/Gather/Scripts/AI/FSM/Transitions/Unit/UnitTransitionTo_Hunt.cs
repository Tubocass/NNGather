using Gather;
using Gather.AI.FSM.States;

namespace Gather.AI.FSM.Transitions
{
    public class UnitTransitionTo_Hunt : FSM_Transition
    {
        EnemyDetector enemyDetector;
        public UnitTransitionTo_Hunt(Blackboard context, FSM_State next) : base(context, next)
        {
            enemyDetector = context.GetValue<EnemyDetector>(Configs.EnemyDetector);
        }

        public override bool IsValid()
        {
            bool hasTarget = context.GetValue<bool>(Configs.HasTarget);
            bool targetIsNearestTarget = true;
            if (hasTarget)
            {
                targetIsNearestTarget = TargetSystem.TargetNearest(unit.GetLocation(), enemyDetector.GetEnemiesList()) == (Unit)context.GetValue<ITargetable>(Configs.Target);
            }
            return !hasTarget || (hasTarget && !targetIsNearestTarget);
        }
    }
}