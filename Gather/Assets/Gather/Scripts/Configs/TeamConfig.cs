using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using gather;

[CreateAssetMenu]
public class TeamConfig : ScriptableObject
{
    [SerializeField] int team;
    [SerializeField] Color teamColor;
    Dictionary<UnitType, Counter> teamCounter = new Dictionary<UnitType, Counter>();
    public enum UnitType { Fighter, Farmer, Queen }
    //public UnitEvent UnitChange;

    public int Team { get => team; }
    public Color TeamColor { get => teamColor; }

    public TeamConfig(int team, Color teamColor)
    {
        this.team = team;
        this.teamColor = teamColor;
    }

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

    public void SetUnitCount(UnitType type, int value)
    {
        if(teamCounter.ContainsKey(type))
        {
            teamCounter[type].AddAmount( value );
        }else
        {
            Counter count = ScriptableObject.CreateInstance<Counter>();
            count.SetAmount( value );
            teamCounter.Add(type, count);
        }
        //UnitChange?.Invoke(type);
    }

    public int GetUnitCount(UnitType type)
    {
        Counter count;
        if(teamCounter.TryGetValue(type, out count))
        {
            return count.amount;
        }
        return 0;
    }

    public Counter GetUnitCounter(UnitType type)
    {
        Counter count;
        if (teamCounter.TryGetValue(type, out count))
        {
            return count;
        }else
        {
            count = ScriptableObject.CreateInstance<Counter>();
            count.SetAmount(0);
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
    }
}
