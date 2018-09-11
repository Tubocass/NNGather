using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;

public class NewLevelGenerator: MonoBehaviour
{
    public int width =64, height =64;
    public bool useRandomSeed = true;
    public string seed;
    [Range(0, 100)]
    public int randomFillPercent;
    int[,] map;
    //use a Scriptable obj to hold all my Prefab refernces.
    [SerializeField] GameObject[] terrainTiles;
    private string gameDataProjectFilePath = "/StreamingAssets/data.json";
    LevelProperties levelProps;

    private void Start()
    {
        Init();
    }
    public void Init()
    {
        LoadGameData();
        map = new int[width, height];
        RandomizeMap();
        for (int i = 0; i < 3; i++)
        {
            SmoothMap();
        }

        CreateTiles();
    }

    private void LoadGameData()
    {
        string filePath = Application.dataPath + gameDataProjectFilePath;

        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            levelProps = JsonUtility.FromJson<LevelProperties>(dataAsJson);
            Debug.Log("File Loaded");
            seed = levelProps.seed;
            useRandomSeed = levelProps.useRandomSeed;
            map = levelProps.map;
        }
        else
        {
            levelProps = new LevelProperties();
        }
    }
    void RandomizeMap()
    {
        System.Random pseudoRandom = new System.Random(seed.GetHashCode());
        Debug.Log(seed.ToString()+ ", is Random?"+ useRandomSeed.ToString());
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                {
                    map[x, y] = 1;
                }
                else
                {
                    map[x, y] = (pseudoRandom.Next(0, 100) < randomFillPercent) ? 1 : 0;
                }
            }
        }
    }
    void CreateTiles()
    {
        GameObject levelTiles = new GameObject("LevelTiles");
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (map[x, y] == 0)
                {
                    GameObject.Instantiate(terrainTiles[0], new Vector3(x, y), Quaternion.identity, levelTiles.transform);
                }
                else
                {
                    GameObject.Instantiate(terrainTiles[1], new Vector3(x, y), Quaternion.identity, levelTiles.transform);
                }
            }
        }
    }
    void SmoothMap()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int neighbourWallTiles = GetSurroundingWallCount(x, y);

                if (neighbourWallTiles > 4)
                    map[x, y] = 1;
                else if (neighbourWallTiles < 4)
                    map[x, y] = 0;

            }
        }
    }

    int GetSurroundingWallCount(int gridX, int gridY)
    {
        int wallCount = 0;
        for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++)
        {
            for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++)
            {
                if (neighbourX >= 0 && neighbourX < width && neighbourY >= 0 && neighbourY < height)
                {
                    if (neighbourX != gridX || neighbourY != gridY)
                    {
                        wallCount += map[neighbourX, neighbourY];
                    }
                }
                else
                {
                    wallCount++;
                }
            }
        }

        return wallCount;
    }
}
