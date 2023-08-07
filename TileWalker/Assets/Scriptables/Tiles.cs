using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Tiles", menuName = "Tile")]
public class Tiles : ScriptableObject
{
    public int row;
    public int col;

    public GameObject tilePrefab;
    public GameObject wallPrefab;

    private Transform tileSpawnTransform;
    private Transform wallSpawnTransform;

    private GameManager gameManager;

    public void Grid()
    {
        tileSpawnTransform = GameObject.Find("Ground").transform;
        wallSpawnTransform = GameObject.Find("Wall").transform;

        gameManager = FindObjectOfType<GameManager>();

        // Calculate the size of each tile based on the tilePrefab's bounds
        Vector3 tileSize = Vector3.zero;
        Renderer tileRenderer = tilePrefab.GetComponent<Renderer>();
        if (tileRenderer != null)
        {
            tileSize = tileRenderer.bounds.size;
        }
        else
        {
            Debug.LogError("Tile Prefab doesn't have a Renderer component.");
            return;
        }

        gameManager.Tile(row, col);
        // This is the method where we will be writing the logic to place the tiles and walls in a grid
        for (int i = 0; i < col; i++)
        {
            for (int j = 0; j < row; j++)
            {

                // Calculate the position for each tile
                Vector3 spawnPosition = new Vector3(i * tileSize.x * 1.2f, -4.5f, j * tileSize.z * 1.2f);
                // Instantiate the tilePrefab at the calculated position
                GameObject newTile = Instantiate(tilePrefab, spawnPosition, Quaternion.identity);
                // Parent the instantiated tile to tileSpawnTransform to keep the Hierarchy organized
                newTile.transform.SetParent(tileSpawnTransform);
                newTile.name = i.ToString() + " " + j.ToString();
                gameManager._grid[i, j] = newTile.gameObject;

                // Create barricades around the entire grid
                if (i == 0 || i == col - 1 || j == 0 || j == row - 1)
                {
                    Vector3 barricadePosition = new Vector3(i * tileSize.x * 1.2f, -4f, j * tileSize.z * 1.2f);
                    var x = Instantiate(wallPrefab, barricadePosition, Quaternion.identity);
                    x.transform.SetParent(wallSpawnTransform);
                }
            }
        }

        // Calculate the center position of the entire grid
        Vector3 gridCenter = new Vector3((col - 1) * tileSize.x * 1.2f * 0.5f, (row + col), (row - 1) * tileSize.z * 1.2f * 0.5f);

        // Position the camera to center on the grid
        Camera.main.transform.position = gridCenter + new Vector3(0f, 2f, -2f);
        Camera.main.transform.LookAt(gridCenter + new Vector3(0f, -20f, 0f));

        Debug.Log("Tiles and walls placed in a proper grid manner.");
    }

    private void DefaultSettings()
    {
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                if(i == 0  || j == 0 || i == row - 1 || j == col - 1)
                {
                    gameManager.SetPlayerPosition(i, j);
                }
            }
        }
    }
}
