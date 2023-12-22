using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSystem
{
    public static List<T> FindTargetsByCount<T>(int amount, string tag, Vector2 location, float distance, LayerMask mask, Predicate<T> boo, out List<T> targets) where T:Component
    {
        targets = new List<T>();

        Collider2D[] cols = Physics2D.OverlapCircleAll(location, distance, mask, -1, 1);
        if (cols.Length > 0)
        {
            for (int f = 0; f < cols.Length; f++)
            {
                if (amount > 0)
                {
                    var comp = cols[f].GetComponent<T>();
                    if (boo.Invoke(comp))
                    {
                        targets.Add(comp);
                        amount--;
                    }
                }
                else break;
            }
        }
        return targets;
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
}
