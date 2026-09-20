using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlatformGenerator : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] private int row;
    [SerializeField] private int column;
    [SerializeField] private int yLevels;
    [SerializeField] private List<Color> levelsColors = new();
    [SerializeField] private Color standPointTilesColor;
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
        return colorIndex >= levelsColors.Count - 1 ? 0 : ++colorIndex;
    }


    public List<Vector3> SetupSpawnTiles(int totalCount)
    {
        List<Vector3> spawnPositions = new List<Vector3>();

        // Calculate center point at top level
        Vector3 centerPos = startPoint.position;
        centerPos.x += spawnOffset.x * (row / 2f);
        centerPos.y += spawnOffset.y * yLevels;
        centerPos.z += spawnOffset.z * (column / 2f);

        // Distance between starting tiles
        float radius = (totalCount > 1) ? spawnOffset.x * 2f : 0f;

        for (int i = 0; i < totalCount; i++)
        {
            Vector3 tilePos = centerPos;

            // Arrange starting tiles in a circle around center
            if (totalCount > 1)
            {
                float angle = i * (360f / totalCount) * Mathf.Deg2Rad;
                tilePos += new Vector3(Mathf.Cos(angle) * radius, 0f, Mathf.Sin(angle) * radius);
            }

            GameObject tile = Instantiate(platformPrefab, tilePos, Quaternion.identity);
            tile.transform.SetParent(transform);
            tile.GetComponent<PlatformTile>().SetPlatformTileColor(standPointTilesColor);
            // Return character spawn position slightly above the tile top surface
            spawnPositions.Add(tilePos + Vector3.up * 0.1f);
        }

        return spawnPositions;
    }
    public Vector3 GetPlatformCenter()
    {
        Vector3 centerPosition = startPoint.position;
        float xOffset = spawnOffset.x * Random.Range(1.0f,3.0f);
        float zOffset = spawnOffset.z * Random.Range(1.0f,3.0f);
        centerPosition.x += xOffset + spawnOffset.x * row/2;
        centerPosition.z += zOffset + spawnOffset.z * column/2;
        return centerPosition;
    }
}
