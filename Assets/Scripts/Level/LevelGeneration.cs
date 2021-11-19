using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public static class LevelGeneration
{
    public static float[,] GenerateTerrain(Vector2Int _gridSize, int _maxHeight, float _scale, float _offset,
        float _randomness, AnimationCurve _rateForTiles, int _smoothness)
    {
        Vector2Int[] shape = TerrainShape(_gridSize);
        (float height, bool done)[,] heightMap = new (float height, bool done)[_gridSize.x, _gridSize.y];
        foreach (Vector2Int i in shape)
        {   //edges of the shape will not be given a height value
            heightMap[i.x, i.y].done = true;
        }

        AddPerlin(heightMap, _scale, _offset, _randomness, _rateForTiles);
        float[,] onlyHeightMap = new float[_gridSize.x, _gridSize.y];
        for (int x = 0; x < _gridSize.x; x++)
        {   //removes the boolean from the height map and puts the height values in the range of the sprite index
            for (int y = 0; y < _gridSize.y; y++)
            {
                onlyHeightMap[x, y] = heightMap[x, y].height * _maxHeight;
                onlyHeightMap[x, y] = Mathf.Clamp(onlyHeightMap[x, y], 0, _maxHeight - 1);
            }
        }

        for (int i = 0; i < _smoothness; i++)
        {
            SmoothHeightMap(onlyHeightMap);
        }

        return onlyHeightMap;
    }

    private static Vector2Int[] TerrainShape(Vector2Int _gridSize)
    {
        //creates a random quadrangle from 4 random points on the grid and drawings raster lines between them
        //returns the index of all edges in the shape
        int minX = _gridSize.x / 20;
        int maxX = _gridSize.x - minX;
        int middleX = (_gridSize.x - 1) / 2;

        int minY = _gridSize.y / 20;
        int maxY = _gridSize.y - minY;
        int middleY = (_gridSize.y - 1) / 2;

        Vector2Int p1 = RandomPointInSection(middleX + minX, maxX, middleY + minY, maxY);
        Vector2Int p2 = RandomPointInSection(middleX + minX, maxX, minY, middleY - minY);
        Vector2Int p3 = RandomPointInSection(minX, middleX - minX, minY, middleY - minY);
        Vector2Int p4 = RandomPointInSection(minX, middleX - minX, middleY + minY, maxY);
        List<Vector2Int> indexesOfShape = new List<Vector2Int>();
        indexesOfShape.AddRange(RasterLine(p1, p2));
        indexesOfShape.AddRange(RasterLine(p2, p3));
        indexesOfShape.AddRange(RasterLine(p3, p4));
        indexesOfShape.AddRange(RasterLine(p4, p1));

        return indexesOfShape.ToArray();

        static Vector2Int[] RasterLine(Vector2Int _a, Vector2Int _b)
        {   //uses dda, should use bresenham's but am lazy and stupid
            int deltaX = _b.x - _a.x;
            int deltaY = _b.y - _a.y;
            int steps = Mathf.Abs(deltaX) > Mathf.Abs(deltaY) ? Mathf.Abs(deltaX) : Mathf.Abs(deltaY);
            float xInc = deltaX / (float) steps;
            float yInc = deltaY / (float) steps;
            float currX = _a.x;
            float currY = _a.y;

            Vector2Int[] tiles = new Vector2Int[steps + 1];
            for (int i = 0; i <= steps; i++)
            {
                tiles[i] = new Vector2Int(Mathf.RoundToInt(currX), Mathf.RoundToInt(currY));
                currX += xInc;
                currY += yInc;
            }

            return tiles;
        }

        static Vector2Int RandomPointInSection(int _xMin, int _xMax, int _yMin, int _yMax)
        {
            int x = Random.Range(_xMin, _xMax);
            int y = Random.Range(_yMin, _yMax);

            return new Vector2Int(x, y);
        }
    }

    private static void AddPerlin((float height, bool done)[,] _heightMap,
        float _scale, float _offset, float _randomness, AnimationCurve _rateForTiles)
    {
        //use flood fill to only apply perlin noise to cells within the generated shape
        int middleX = (_heightMap.GetLength(0) - 1) / 2;
        int middleY = (_heightMap.GetLength(1) - 1) / 2;

        Stack<Vector2Int> tilesToFlood = new Stack<Vector2Int>();
        tilesToFlood.Push(new Vector2Int(middleX, middleY));

        while (tilesToFlood.Count > 0)
        {
            Vector2Int coords = tilesToFlood.Pop();
            (float height, bool done) currHeight = _heightMap[coords.x, coords.y];
            if (!currHeight.done)
            {
                float praline = Mathf.PerlinNoise(coords.x * _scale + _offset,
                    coords.y * _scale + _offset);
                praline += Random.Range(-_randomness, _randomness);
                praline = _rateForTiles.Evaluate(praline);
                _heightMap[coords.x, coords.y].height = praline;
                _heightMap[coords.x, coords.y].done = true;

                tilesToFlood.Push(new Vector2Int(coords.x + 1, coords.y)); //check closed before adding
                tilesToFlood.Push(new Vector2Int(coords.x - 1, coords.y));
                tilesToFlood.Push(new Vector2Int(coords.x, coords.y + 1));
                tilesToFlood.Push(new Vector2Int(coords.x, coords.y - 1));
            }
        }
    }

    private static void SmoothHeightMap(float[,] _heightMap)
    {
        //smooth by surrounding cells in the grid
        int width = _heightMap.GetLength(0);
        int height = _heightMap.GetLength(1);
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float totSurroundingHeight = 0;
                int surroundingAmount = 0;
                for (int circleX = -1; circleX < 1; circleX++)
                {
                    for (int circleY = -1; circleY < 1; circleY++)
                    {
                        if (x + circleX < 0 || x + circleX >= width || y + circleY < 0 || y + circleY >= height)
                            continue;

                        totSurroundingHeight += _heightMap[x + circleX, y + circleY];
                        surroundingAmount++;
                    }
                }

                _heightMap[x, y] = totSurroundingHeight / surroundingAmount;
            }
        }
    }
}