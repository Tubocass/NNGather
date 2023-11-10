using TMPro;
using UnityEngine;

public class StatHolder : MonoBehaviour
{
    [SerializeField] private TMP_Text _label;
    [SerializeField] private TMP_Text _value;

    private StatData _statData;
    private Stats _stats;

    public void SetData(Stats stats, StatData statData)
    {
        _stats = stats;
        _statData = statData;
        _label.SetText(statData.StatType.ToString());
        _value.SetText(statData.Value.ToString());
    }

    public void AddStat()
    {
        _stats.Modify(_statData.StatType, 1);
    }
}