using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandGenerator : MonoBehaviour {

	public GameObject islandBasePrefab;
	public GameObject[] tilePrefabs;
	public RandomValue tileQuantityRange;

	public GameObject GenerateIsland()
	{
		GameObject islandObj = Instantiate(islandBasePrefab);
		Island island = islandObj.GetComponent<Island>();
		int chosenTileIndex = Random.Range(0, tilePrefabs.Length);
		Tile chosenTile = Instantiate(tilePrefabs[chosenTileIndex], islandObj.transform).GetComponent<Tile>();
		chosenTile.SetIsland(island);
		chosenTile.SetSortingOrder();
		island.tiles.Add(Vector2.zero, chosenTile);
		int tileQuantity = (int)tileQuantityRange.GetRandom();
		while (island.tiles.Count < tileQuantity) {
			List<Vector2> tileList = new List<Vector2>(island.tiles.Keys);
			Vector2 randTileVector = tileList[Random.Range(0, tileList.Count)];
			Tile existingTile = island.tiles[randTileVector];
			Vector2 newTileVect = GetRandAdjacentVector(randTileVector);
			if (!island.tiles.ContainsKey(newTileVect)) {
				// Initialize tile
				Tile newTile = Instantiate(tilePrefabs[chosenTileIndex], Vector2.zero, Quaternion.identity, islandObj.transform).GetComponent<Tile>();
				newTile.SetIsland(island);
				// Move tile to position in island
				newTile.transform.position += new Vector3(newTileVect.x * newTile.GetComponent<Collider2D>().bounds.size.x, newTileVect.y * newTile.GetComponent<Collider2D>().bounds.size.y, 0);
				newTile.SetSortingOrder();
				//Debug.Log("tile at " + newTileVect + " at sorting order " + newTile.GetSortingOrder());
				island.tiles.Add(newTileVect, newTile);
			}
		}
		//TODO: resize box collider
		//TODO: fix sorting order
		return islandObj;
	}

	private Vector2 GetRandAdjacentVector(Vector2 vector)
	{
		if (Random.value < 0.25f) {
			return vector + Vector2.up;
		} else if (Random.value < 0.5f) {
			return vector + Vector2.down;
		} else if (Random.value < 0.75f) {
			return vector + Vector2.left;
		} else {
			return vector + Vector2.right;
		}
	}
}
