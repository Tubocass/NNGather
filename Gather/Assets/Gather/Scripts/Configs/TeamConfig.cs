using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace gather
{
    [CreateAssetMenu]
    public class TeamConfig : ScriptableObject
    {
        [SerializeField] public int Team;
        [SerializeField] public Color TeamColor;
        Dictionary<UnitType, Counter> teamCounter = new Dictionary<UnitType, Counter>();
        Dictionary<int, int> foodTargets = new Dictionary<int, int>();

        //public int Team { get => team; }
        //public Color TeamColor { get => teamColor; }

        private void OnEnable()
        {
            teamCounter = new Dictionary<UnitType, Counter>();
        }

        private void OnDisable()
        {
            Reset();
        }

        private void OnDestroy()
        {
            Reset();
        }

        public void TargetFood(int droneID, int foodID)
        {
            if(!foodTargets.ContainsKey(foodID))
            {
                foodTargets.Add(foodID, droneID);
            }
        }

        public bool CanTargetFood(int droneID, int foodID)
        {
            int heldID;
            if(foodTargets.TryGetValue(foodID, out heldID))
            {
                return heldID == droneID;
            }else return true;
        }

        public void UntargetFood(int foodID)
        {
            if (foodTargets.ContainsKey(foodID))
            {
                foodTargets.Remove(foodID);
            }
        }

        public void UpdateUnitCount(UnitType type, int value)
        {
            if (teamCounter.ContainsKey(type))
            {
                teamCounter[type].AddAmount(value);
            }
            else
            {
                Counter count = ScriptableObject.CreateInstance<Counter>();
                count.Amount = value;
                teamCounter.Add(type, count);
            }
        }

        public int GetUnitCount(UnitType type)
        {
            Counter count;
            if (teamCounter.TryGetValue(type, out count))
            {
                return count.Amount;
            }
            return 0;
        }

        public Counter GetUnitCounter(UnitType type)
        {
            Counter count;
            if (teamCounter.TryGetValue(type, out count))
            {
                return count;
            }
            else
            {
                count = ScriptableObject.CreateInstance<Counter>();
                count.Amount = 0;
                teamCounter.Add(type, count);
                return count;
            }
        }

        public int GetTeamCount()
        {
            int teamCount = 0;
            var keys = teamCounter.Keys;
            foreach (var key in keys)
            {
                teamCount += GetUnitCount(key);
            }
            return teamCount;
        }

        public void Reset()
        {
            this.teamCounter.Clear();
            foodTargets.Clear();
        }
    }
}
