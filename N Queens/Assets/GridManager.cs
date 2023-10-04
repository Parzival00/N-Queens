using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    [SerializeField] int size;

    [SerializeField] private Tile tilePrefab;

    [SerializeField] private Transform cam;
    //[SerializeField] InputField sizeInput;

    private Dictionary<Vector2, Tile> tiles;

    public void generateBoth()
    {
        size = 8;
        generateGrid();
        generateGrid2();
    }
    public void generateGrid()
    {
        for(int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                var spawnedTile = Instantiate(tilePrefab, new Vector3(x/2f, y/2f), Quaternion.identity);
                spawnedTile.name = $"Tile {x} {y}";

                var isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
                spawnedTile.Init(isOffset);

                //tiles[new Vector2(x, y)] = spawnedTile;
            }
        }
        //cam.transform.position = new Vector3((float) size/1.25f - 0.5f, (float)size / 4f - 0.5f,-10);
    }

    public void generateGrid2()
    {
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                var spawnedTile = Instantiate(tilePrefab, new Vector3(x / 2f + (10), y / 2f), Quaternion.identity);
                spawnedTile.name = $"Tile2 {x} {y}";

                var isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
                spawnedTile.Init(isOffset);

                //tiles[new Vector2(x, y)] = spawnedTile;
            }
        }
        //cam.transform.position = new Vector3((float)width / 1.25f - 0.5f, (float)height / 4f - 0.5f, -10);
    }

    public Tile getTileAtPosition(Vector2 pos)
    {
        if(tiles.TryGetValue(pos, out var tile))
        {
            return tile;
        }
        return null;
    }

    public Transform getTilePosition(int x, int y)
    {
        Tile[] tiles = FindObjectsOfType<Tile>();
        foreach(Tile tile in tiles)
        {
            if (tile.name == ($"Tile {x} {y}"))
            {
                //print("found");
                return tile.transform;
            }
        }

        return null;
    }

    public Transform getTilePosition2(int x, int y)
    {
        Tile[] tiles = FindObjectsOfType<Tile>();
        foreach (Tile tile in tiles)
        {
            if (tile.name == ($"Tile2 {x} {y}"))
            {
                //print("found");
                return tile.transform;
            }
        }

        return null;
    }
}
