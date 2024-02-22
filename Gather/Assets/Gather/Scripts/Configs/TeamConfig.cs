using UnityEngine;
using Gather.AI;

namespace gather
{
    [CreateAssetMenu]
    public class TeamConfig : ScriptableObject
    {
        [SerializeField] public int TeamID;
        [SerializeField] public Color TeamColor;
        UnitManager unitManager;
        FoodManager foodManager;
        Blackboard teamContext = new Blackboard();

        public UnitManager UnitManager => unitManager;
        public FoodManager FoodManager => foodManager;

        private void OnEnable()
        {
            unitManager = new UnitManager(teamContext);
            foodManager = new FoodManager(teamContext);
        }
    }
}
