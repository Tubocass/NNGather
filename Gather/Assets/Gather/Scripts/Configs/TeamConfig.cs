using System.Collections.Generic;
using UnityEngine;

namespace gather
{
    [CreateAssetMenu]
    public class TeamConfig : ScriptableObject
    {
        [SerializeField] public int Team;
        [SerializeField] public Color TeamColor;
        UnitManager unitManager;
        Dictionary<int, int> foodTargets = new Dictionary<int, int>();

        private void OnEnable()
        {
            unitManager = new UnitManager();
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
            unitManager.UpdateUnitCount(type, value);
        }

        public Counter GetUnitCounter(UnitType type)
        {
            return unitManager.GetUnitCounter(type);
        }

        public int GetTeamCount()
        {
            return unitManager.GetTeamCount();
        }

        public void Reset()
        {
            foodTargets.Clear();
        }
    }
}
