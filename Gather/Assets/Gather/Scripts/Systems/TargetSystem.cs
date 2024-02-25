using System;
using System.Collections.Generic;
using UnityEngine;

public class TargetSystem
{
    public static void FindTargetsByCount<T>(List<T> targets, Vector2 location, SearchConfig config, Predicate<T> boo) where T:Component
    {
        targets.Clear();
        Collider2D[] cols = Physics2D.OverlapCircleAll(location, config.searchDist, config.searchLayer);
        int amount = config.searchAmount;
        if (cols.Length > 0)
        {
            for (int f = 0; f < cols.Length; f++)
            {
                if (amount > 0)
                {
                    var comp = cols[f].GetComponent<T>();
                    if (comp && boo.Invoke(comp))
                    {
                        targets.Add(comp);
                        amount--;
                    }
                }
                else break;
            }
        }
    }
    
    public static T TargetNearest<T>(Vector3 position, List<T> targets) where T : Component
    {
        float nearestDist, newDist;
        int targetIndex = 0;
        T nearestTarget = null;

        if (targets.Count <= 0)
        {
            return null;
        }else
        {
            nearestDist = (targets[0].transform.position - position).sqrMagnitude; //compare the squared distances
            for (int f = 0; f < targets.Count; f++)
            {
                if (targets[f] != null && targets[f].gameObject.activeSelf)
                {
                    newDist = (targets[f].transform.position - position).sqrMagnitude;//compare the squared distances
                    if (newDist <= nearestDist)
                    {
                        nearestDist = newDist;
                        targetIndex = f;
                    }
                }
            }
            nearestTarget = targets[targetIndex].GetComponent<T>();
        }

        return nearestTarget;
    }

    public static Vector2 TargetNearest(Vector2 position, List<Vector2> targets)
    {
        float nearestDist, newDist;
        int targetIndex = 0;

        if (targets.Count <= 0)
        {
            return Vector2.zero;
        } else
        {
            nearestDist = (targets[0] - position).sqrMagnitude; //compare the squared distances
            for (int f = 0; f < targets.Count; f++)
            {
                if (targets[f] != Vector2.zero)
                {
                    newDist = (targets[f] - position).sqrMagnitude;//compare the squared distances
                    if (newDist <= nearestDist)
                    {
                        nearestDist = newDist;
                        targetIndex = f;
                    }
                }
            }
        }

        return targets[targetIndex];
    }
}
