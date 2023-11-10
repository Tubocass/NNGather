using UnityEngine;

public class StatAdder : MonoBehaviour
{
    [SerializeField] private KeyCode _addStatKey = KeyCode.Space;
    [SerializeField] private StatType _addStatType;
    [SerializeField] private int _addStatAmount;
    private Stats _stats;

    private void Awake()
    {
        _stats = GetComponent<Stats>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(_addStatKey))
        {
            _stats.Modify(_addStatType, _addStatAmount);
        }
    }
}