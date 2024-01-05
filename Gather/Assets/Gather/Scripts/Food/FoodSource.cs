﻿using System.Collections;
using System;
using UnityEngine;

namespace gather
{
    public class FoodSource : MonoBehaviour
    {
        Vector2[] spawnPositions;
        FoodFactory foodFactory;
        [SerializeField] int spawnRangeDist = 4;
        [SerializeField] float timer = 5f;
        [SerializeField] bool isTimeVariable;
        [SerializeField] float variableTimeAmount = 1f;
        [SerializeField] float minimumDistance = 2f;
        [SerializeField] int baseFood, variableFood;
        Vector2 center { get => (Vector2)transform.position;  }

        /*
         *  Would like to see these random variables under a Normal curve
         */

        private void Awake()
        {
            foodFactory = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<FoodFactory>();
            int numPositions = UnityEngine.Random.Range(1, variableFood) + baseFood;
            spawnPositions = new Vector2[numPositions];
            Vector2 pos = Vector2.zero;
            for (int p = 0; p < spawnPositions.Length; p++)
            {
                do
                {
                    pos = UnityEngine.Random.insideUnitCircle * spawnRangeDist;
                } while (!MinimumDistanceCheck(pos, p));
                
                spawnPositions[p] = pos;
            }

            if (isTimeVariable)
            {
                float varTime = UnityEngine.Random.Range(-variableTimeAmount, variableTimeAmount);
                timer += varTime;
            }
            SpawnAllAtOnce();
            StartCoroutine(Respawn());
        }

        IEnumerator Respawn()
        {
            for (int p = 0; p < spawnPositions.Length; p++)
            {
                yield return new WaitForSeconds(timer);

                if (!Physics2D.OverlapPoint(center + spawnPositions[p], MaskLayers.food))
                {
                    foodFactory.Spawn(center + spawnPositions[p])
                        .transform.SetParent(this.transform);
                }
            }
        }

        void SpawnAllAtOnce()
        {
            for (int p = 0; p < spawnPositions.Length; p++)
            {
                foodFactory.Spawn(center + spawnPositions[p])
                       .transform.SetParent(this.transform);
            }
        }

        bool MinimumDistanceCheck(Vector2 point, int length)
        {
            if (length == 0)
            {
                return true;
            }
            for (int p = 0; p < length; p++)
            {
                if (Vector2.Distance(point, spawnPositions[p]) < minimumDistance)
                {
                    return false;
                }
            }
            return true;
        }
    }
}