using UnityEngine;

namespace Gather
{
    public delegate void LocationEvent(Vector2 location);
    public delegate void FoodEvent(int amount);
    public delegate void GameEvent();
    public delegate void StatusEvent(bool status);
    public delegate void TargetEvent();
    //public delegate void UnitEvent(TeamConfig.UnitType unitType);
    //public delegate void TargetEvent(Unit target);

    public static class EventTypes
    {
    }
}