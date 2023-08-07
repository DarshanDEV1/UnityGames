using UnityEngine;
using DT_UI;
using System.Collections.Generic;
using System.Collections;
using System.Threading;

[System.Serializable]
public class GameManager : MonoBehaviour
{
    [SerializeField] UIManager _ui_Manager;
    [SerializeField] Tiles _tiles;
    private int row;
    private int col;
    public GameObject[,] _grid;
    public GameObject playerPrefab;

    [SerializeField] PlayerPosition _playerPosition;
    [SerializeField] List<PlayerVisited> _visitedTiles = new List<PlayerVisited>();

    private void Awake()
    {
        //Getting the property from the hierarchy in the runtime.
        _ui_Manager = FindObjectOfType<UIManager>();
    }

    [System.Obsolete]
    private void Start()
    {
        _tiles.Grid();
        PlayerSpawn();
        StartCoroutine(SpawnDangerTile(true));
        StartCoroutine(SpawnDonut(true));
    }

    public void Tile(int row, int col)
    {
        _grid = new GameObject[row, col];
        this.row = row;
        this.col = col;
    } //Gets the grid of tiles from the scriptable object;

    private void PlayerSpawn()
    {
        int x = Random.Range(0, row);
        int y = Random.Range(0, col);

        _playerPosition = new PlayerPosition { row = x, col = y };

        for (int i = 0; i < this.row; i++)
        {
            for (int j = 0; j < this.col; j++)
            {
                if (i == x && j == y)
                {
                    var player = Instantiate(playerPrefab);
                    player.transform.position = _grid[i, j].transform.position;
                    _visitedTiles.Add(new PlayerVisited { row = i, col = j });
                    TileMarker();
                }
            }
        }
    } //Spawns the player in a random tile;

    public void TileMarker()
    {
        foreach (PlayerVisited _visited in _visitedTiles)
        {
            int row = _visited.row;
            int col = _visited.col;
            Renderer render = _grid[row, col].GetComponent<Renderer>();
            Material material = render.material;
            material.color = Color.green;
        }
    } //Fetches all the visited tiles from the list and mark them green color;

    private int TileChecker(int row, int col)
    {
        Renderer render = _grid[row, col].GetComponent<Renderer>();
        Material material = render.material;
        Color color = material.color;

        if (color == Color.red)
        {
            return -1;
        }
        else if (color == Color.green)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    } //Checks the tile and returns red = -1, green = 1, normal = 0;

    [System.Obsolete]
    private bool DonutChecker(int row, int col)
    {
        if (_grid[row, col].transform.GetChild(0).gameObject.active)
        {
            return true;
        }
        return false;
    } //Returns true if donut is active in that pos else false

    [System.Obsolete]
    private TilePosition GetDonutLocation(int row, int col)
    {
        if (TileChecker(row, col) == 1 || TileChecker(row, col) == -1 || DonutChecker(row, col))
        {
            row = Random.Range(0, this.row);
            col = Random.Range(0, this.col);
            GetDonutLocation(row, col);
        }
        return new TilePosition { row = row, col = col };
    }

    [System.Obsolete]
    private IEnumerator SpawnDonut(bool config)
    {
        while (config)
        {

            //yield return new WaitForSeconds(6f);
            int x = Random.Range(0, this.row);
            int y = Random.Range(0, this.col);
            TilePosition donutLocation = GetDonutLocation(x, y);
            _grid[donutLocation.row, donutLocation.col].transform.GetChild(0).gameObject.SetActive(true);
            yield return new WaitForSeconds(4f);
            _grid[donutLocation.row, donutLocation.col].transform.GetChild(0).gameObject.SetActive(false);
        }

    } //Spawns donut in random tile in a time interval of 3seconds;

    [System.Obsolete]
    private IEnumerator SpawnDangerTile(bool config)
    {
        while (config)
        {
            for (int iteration = 0; iteration < 3; iteration++) // Repeat the process three times
            {
                List<TilePosition> enabledTilePositions = new List<TilePosition>();

                // Enable three tiles at a time
                for (int i = 0; i < 3; i++)
                {
                    int x = Random.Range(0, this.row);
                    int y = Random.Range(0, this.col);
                    TilePosition donutLocation = GetDonutLocation(x, y);
                    Renderer render = _grid[donutLocation.row, donutLocation.col].GetComponent<Renderer>();
                    Material material = render.material;
                    material.color = Color.red;
                    enabledTilePositions.Add(donutLocation);
                }

                yield return new WaitForSeconds(4f);

                // Revert the enabled tiles to their normal state
                foreach (var position in enabledTilePositions)
                {
                    Renderer render = _grid[position.row, position.col].GetComponent<Renderer>();
                    Material material = render.material;
                    material.color = Color.blue;
                }
            }

            // Add additional delay before the next iteration (optional)
            yield return new WaitForSeconds(4f);
        }
    }

    public PlayerPosition GetPlayerPostition()
    {
        return _playerPosition;
    }

    public void SetPlayerPosition(int row, int col)
    {
        _playerPosition = new PlayerPosition { row = row, col = col };
        _visitedTiles.Add(new PlayerVisited { row = row, col = col });
        TileMarker();
    }

    public TilePosition GetCoordinates(string gameObjectName)
    {
        for (int i = 0; i < this.row; i++)
        {
            for (int j = 0; j < this.col; j++)
            {
                if (_grid[i, j].name == gameObjectName)
                {
                    return new TilePosition { row = i, col = j };
                }
            }
        }
        return new TilePosition { row = 0, col = 0 };
    }
}

[System.Serializable]
public struct PlayerPosition
{
    public int row;
    public int col;
}
[System.Serializable]
public struct PlayerVisited
{
    public int row;
    public int col;
}
[System.Serializable]
public struct TilePosition
{
    public int row;
    public int col;
}