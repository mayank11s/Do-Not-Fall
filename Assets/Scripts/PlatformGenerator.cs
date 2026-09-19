using System;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] private int row;
    [SerializeField] private int column;
    [SerializeField] private int yLevels;
    [SerializeField] List<Color> levelsColors = new();
    [SerializeField] private Vector3 spawnOffset;
    [SerializeField] private float evenRowOffset = 0.8725f;
    [SerializeField] private Transform startPoint;
    [SerializeField] private GameObject platformPrefab;

    public void Init()
    {
        CheckColors();
        GenerateGrid();
    }
    void CheckColors()
    {
        if (levelsColors.Count <= 0)
        {
            levelsColors.Add(Color.red);
            levelsColors.Add(Color.blue);
            levelsColors.Add(Color.yellow);
            levelsColors.Add(Color.green);
        }
    }
    private void GenerateGrid()
    {
        Vector3 spawnPosition = startPoint.position;

        int colorIndex = 0;
        Color currentLevelColor = levelsColors[colorIndex];

        for (int y = 0; y < yLevels; y++)
        {
            for (int x = 0; x < row; x++)
            {
                for (int z = 0; z < column; z++)
                {
                    GameObject platformTile = Instantiate(platformPrefab, spawnPosition, Quaternion.identity);
                    platformTile.GetComponent<PlatformTile>().SetPlatformTileColor(currentLevelColor);
                    platformTile.transform.SetParent(transform);
                    spawnPosition.z += spawnOffset.z;
                }
                spawnPosition.z = startPoint.position.z;
                if (x % 2 == 0)
                    spawnPosition.z += evenRowOffset;
                spawnPosition.x += spawnOffset.x;
            }

            spawnPosition.x = startPoint.position.x;
            spawnPosition.y += spawnOffset.y;

            colorIndex = GetNextLevelColor(colorIndex);
            currentLevelColor = levelsColors[colorIndex];
        }

    }
    private int GetNextLevelColor(int colorIndex)
    {
        return colorIndex >= levelsColors.Count -1 ? 0: ++colorIndex;
    }


    public Vector3 SetupPlayerStandTile()
    {
        Vector3 position = startPoint.position;
        position.x += spawnOffset.x * row/2;
        position.y += spawnOffset.y * yLevels;
        position.z += spawnOffset.z * column/2;

        GameObject tile = Instantiate(platformPrefab,position,Quaternion.identity);
        tile.transform.SetParent(transform);

        return position;
    }
}
