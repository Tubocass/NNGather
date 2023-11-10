using System;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public Stats Stats { get; private set; }
    public int Health { get; private set; }
    public event Action<int> OnHealthChanged;

    public void TakeDamage(int amount)
    {
        Health -= amount;
        OnHealthChanged?.Invoke(Health);
    }

    private void Awake()
    {
        Stats = GetComponent<Stats>();
        Health = 100;
    }
}