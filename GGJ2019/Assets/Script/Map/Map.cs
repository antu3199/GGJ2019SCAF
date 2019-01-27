using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BackgroundSpawn {
    public GameObject background;
    public int freq;
}

public class Map : MonoBehaviour {
    public int rows;
    public int cols;
    private Tile[,] tiles;

    public GameObject GhostIsland;
    public GameObject StarterIsland;
    public GameObject PlayerSuite;
    public CameraBehaviour cam;

    public IslandSpawner islandSpawner;

    public List<BackgroundSpawn> backgroundSpawn;
    public float flickerSpeed;
    public Vector2 tileSize;

    void Start() {
        tiles = new Tile[rows, cols];
    	InitializeMap();
        SpawnBackground();
        SpawnStartIsland();
        SpawnPlayer();
    }

    public Vector3 CoordToPosition(int x, int y)
    {
        return new Vector3(x * tileSize.x, y * tileSize.y, 0) + gameObject.transform.position;
    }

	public Vector3 CoordToPosition(Vector2 coord)
	{
		return new Vector3(coord.x * tileSize.x, coord.y * tileSize.y, 0) + gameObject.transform.position;
	}

	public Tile GetTile(int x, int y) {
        return tiles[x, y];
    }

    public Tile GetTile(Vector2 coord) {
        return tiles[(int)coord.x, (int)coord.y];
    }

    public bool CheckCoord(int x, int y) {
        return (x >= 0 && x < rows &&
            y >= 0 && y < cols);
    }

    public bool CheckCoord(Vector2 coord) {
        int x = (int)coord.x;
        int y = (int)coord.y;

        return (x >= 0 && x < rows &&
            y >= 0 && y < cols);
    }

    public Island SpawnIsland(Island island, int x, int y) {
        Island newIsland = Instantiate(island.gameObject, gameObject.transform).GetComponent<Island>();
        newIsland.coordinate = new Vector2(x, y);
        newIsland.type = IslandType.GridLocked;
        newIsland.map = this;
        foreach(KeyValuePair<Vector2, Tile> pair in newIsland.tiles) {
            PlaceTile(pair.Value, x + (int)pair.Key.x, y + (int)pair.Key.y);
        }
        
        return newIsland;
    }

    public Tile SpawnTile(Tile tile, int x, int y) {
        Tile replacedTile = this.tiles[x, y];        
        this.tiles[x, y] = Instantiate(tile.gameObject, gameObject.transform).GetComponent<Tile>();
        this.tiles[x, y].coordinate = new Vector2(x, y);
        this.tiles[x, y].transform.position = CoordToPosition(x, y);
        this.tiles[x, y].SetSortingOrder();
        if (replacedTile != null) {
            replacedTile.islandRef.island.RemoveTile(new Vector2(x, y));
            Destroy(replacedTile.gameObject);
        }

        return this.tiles[x, y];
    }

    public void PlaceTile(Tile tile, int x, int y)
    {
        Tile replacedTile = this.tiles[x, y];        
        this.tiles[x, y] = tile;
        this.tiles[x, y].coordinate = new Vector2(x, y);
        this.tiles[x, y].transform.position = CoordToPosition(x, y);
        this.tiles[x, y].SetSortingOrder();
        if (replacedTile != null) {
            replacedTile.islandRef.island.RemoveTile(new Vector2(x, y));
            Destroy(replacedTile.gameObject);
        }
    }

    private void InitializeMap() {
    	for(int i = 0; i < rows; i++) {
    		for(int j = 0; j < cols; j++) {
                SpawnIsland(GhostIsland.GetComponent<Island>(), i, j);
    		}
    	}	
    }

    private void SpawnBackground() {
        List<GameObject> randomizer = new List<GameObject>();
        foreach(BackgroundSpawn background in backgroundSpawn) {
            for(int i = 0; i < background.freq; i++) {
                randomizer.Add(background.background);
            }
        }

        for(int i = 0; i < rows; i++) {
            for(int j = 0; j < cols; j++) {
                Stars back = Instantiate(randomizer[Random.Range(0, randomizer.Count)], gameObject.transform).GetComponent<Stars>();
                back.transform.position = CoordToPosition(i, j);
                back.flickerFreq = flickerSpeed;
            }
        }
    }

    private void SpawnStartIsland() {
        islandSpawner.homeIsland = SpawnIsland(StarterIsland.GetComponent<Island>(), rows/2, cols/2).gameObject;
    }

    private void SpawnPlayer() {
        GameObject suite = Instantiate(PlayerSuite, gameObject.transform);
        Player player = suite.GetComponentInChildren<Player>();
        Vector2 location = CoordToPosition(rows/2, cols/2);
        player.transform.position = new Vector3(location.x, location.y, tiles[rows/2, cols/2].GetSortingOrder());
        suite.transform.parent = null;
        cam.player = player;
    }
}