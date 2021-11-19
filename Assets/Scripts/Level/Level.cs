using UnityEngine;

public class Level : MonoBehaviour
{
    public static Level current;

    [SerializeField] private float cellSize = 1f;
    [SerializeField] private Vector2Int gridSize = new Vector2Int(10, 10);
    [SerializeField] private AnimationCurve rateForTerrain;
    [SerializeField, Range(0f,1f)] private float random;
    [SerializeField] private float scale;
    [SerializeField] private int smoothness;
    [SerializeField] private Sprite[] terrainSprites;

    public GridCell[,] LevelGrid { get; private set; }
    
    public class GridCell
    {
        public Vector2Int index;
        public Vector2 position;
        public ICellOccupant occupant;
    }

    public GridCell RandomEmptyCell()
    {
        GridCell randomCell = LevelGrid[0, 0];
        for (int i = 0; i < gridSize.x * gridSize.y * 1.6667f; i++)
        {
            int x = Random.Range(0, gridSize.x - 1);
            int y = Random.Range(0, gridSize.y - 1);
            randomCell = LevelGrid[x, y];
            if (randomCell.occupant == null) break;
        }

        return randomCell;
    }

    public bool WithinGrid(Vector2Int _index)
        => _index.x < gridSize.x && _index.x >= 0 && _index.y < gridSize.y && _index.y >= 0;

    private void PopulateLevelGrid()
    {
        float[,] heightMap = LevelGeneration.GenerateTerrain(gridSize, terrainSprites.Length, scale, 
            Random.Range(-1000f, 1000f), random, rateForTerrain, smoothness);

        LevelGrid = new GridCell[gridSize.x, gridSize.y];
        Vector2 half = new Vector2(gridSize.x / 2f * cellSize, gridSize.x / 2f * cellSize);
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                //cells have their position in the middle because the pivot of all game objects on the grid have their pivot in the middle
                //subtract by half so the grid starts in the bottom left instead of the middle of the screen
                LevelGrid[x, y] = new GridCell
                {
                    position = new Vector2(cellSize * x + cellSize / 2f, cellSize * y + cellSize / 2f) - half,
                    index = new Vector2Int(x, y)
                };
                SpawnCell(LevelGrid[x, y], Mathf.RoundToInt(heightMap[x, y]));
            }
        }
    }

    private void SpawnCell(GridCell _cell, int _height)
    {
        GameObject newObj = new GameObject
        {
            transform =
            {
                position = _cell.position,
                parent = transform
            },
            name = "TerrainCell"
        };

        SpriteRenderer spriteRenderer = newObj.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = terrainSprites[_height];
        spriteRenderer.sortingOrder = 1;

        if (_height == 0) //water kills snakes obviously
            _cell.occupant = Obstacle.current;
    }

    private void Awake()
    {
        current = this;
        PopulateLevelGrid();
    }
}