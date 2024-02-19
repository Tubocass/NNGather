using UnityEngine;

namespace gather
{
    [CreateAssetMenu]
    public class TeamConfig : ScriptableObject
    {
        [SerializeField] public int TeamID;
        [SerializeField] public Color TeamColor;
        UnitManager unitManager;
        FoodManager foodManager;

        public UnitManager UnitManager => unitManager;
        public FoodManager FoodManager => foodManager;

        private void OnEnable()
        {
            unitManager = new UnitManager();
            foodManager = new FoodManager();
        }
    }
}
