using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class Stats : MonoBehaviour
{
    public event Action<StatData> OnStatChanged;
    
    [SerializeField] private StatData[] _statDatas;

    public List<StatData> RuntimeStatValues;
    
    private void Awake()
    {
        RuntimeStatValues = new List<StatData>();
        foreach (var statData in _statDatas)
        {
            RuntimeStatValues.Add(new StatData() {StatType = statData.StatType, Value = statData.Value});
        }
    }

    public void Modify(StatType statType, int amount)
    {
        var statData = RuntimeStatValues.FirstOrDefault(t => t.StatType == statType);
        if (statData == null)
        {
            statData = new StatData() {StatType = statType };
            RuntimeStatValues.Add(statData);
        }

        statData.Value += amount;
        OnStatChanged?.Invoke(statData);
    }
}