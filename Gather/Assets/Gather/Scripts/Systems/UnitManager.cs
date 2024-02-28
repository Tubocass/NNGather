using Gather.AI;
using System.Collections.Generic;
using UnityEngine;

namespace Gather
{
    public class UnitManager 
    {
        Dictionary<UnitType, Counter> unitCounter = new Dictionary<UnitType, Counter>();
        private Blackboard teamContext;

        public UnitManager(Blackboard teamContext)
        {
            this.teamContext = teamContext;
        }

        public void UpdateUnitCount(UnitType type, int value)
        {
            GetUnitCounter(type).AddAmount(value);
        }

        public Counter GetUnitCounter(UnitType type)
        {
            Counter count;
            if (unitCounter.TryGetValue(type, out count))
            {
                return count;
            } else
            {
                count = ScriptableObject.CreateInstance<Counter>();
                count.SetAmount(0);
                unitCounter.Add(type, count);
                return count;
            }
        }

        public int GetTeamCount()
        {
            int teamCount = 0;
            var keys = unitCounter.Keys;
            foreach (var key in keys)
            {
                teamCount += GetUnitCounter(key).GetAmount();
            }
            return teamCount;
        }
    }
}