using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class IslandGenerator : MonoBehaviour {

	public GameObject islandBasePrefab;
	public TileInfo[] tileInfo;
	public float entitySpawnChance;         // Chance of an individual tile having an entity.
	public RandomValue tileQuantityRange;
	public float islandLifetime;            // Duration in seconds that an island lasts before destroying itself.
	public GameObject[] npcPrefabs;
	public float npcChance;                 // Chance of an island having an npc.
	public RandomValue npcQuantity;

	public static void RandomizeDecor(Tile tile) {
		Decor[] decors = tile.gameObject.GetComponentsInChildren<Decor>();
		foreach (Decor decor in decors) {
			decor.RandomizeDecor();
		}
	}

	public GameObject GenerateIsland()
	{
		GameObject islandObj = Instantiate(islandBasePrefab);
		Island island = islandObj.GetComponent<Island>();

		// Add tiles
		GrowTiles(island);

		// Add NPCs
		if(Random.value < npcChance) {
			PlaceNpcs(island);
		}

		// Give islands different SortingGroup orders
		islandObj.GetComponent<SortingGroup>().sortingOrder = Random.Range(0, 1000);
		StartCoroutine(island.BeginTimeout(islandLifetime));
		return islandObj;
	}

	// Create shape of island by generating tiles and the entities on top of them.
	private void GrowTiles(Island island)
	{
		int chosenTileIndex = Random.Range(0, tileInfo.Length);
		TileInfo chosenTileInfo = tileInfo[chosenTileIndex];
		Tile chosenTile = Instantiate(chosenTileInfo.tilePrefab, island.gameObject.transform).GetComponent<Tile>();
		RandomizeDecor(chosenTile);
		chosenTile.SetIsland(island, Vector2.zero);
		chosenTile.SetSortingOrder();
		island.tiles.Add(Vector2.zero, chosenTile);
		int tileQuantity = (int)tileQuantityRange.GetRandom();
		while (island.tiles.Count < tileQuantity)
		{
			List<Vector2> tileList = new List<Vector2>(island.tiles.Keys);
			Vector2 randTileVector = tileList[Random.Range(0, tileList.Count)];
			Tile existingTile = island.tiles[randTileVector];
			Vector2 newTileVect = GetRandAdjacentVector(randTileVector);
			if (!island.tiles.ContainsKey(newTileVect))
			{
				// Initialize tile
				Tile newTile = Instantiate(chosenTileInfo.tilePrefab, Vector2.zero, Quaternion.identity, island.gameObject.transform).GetComponent<Tile>();
				RandomizeDecor(newTile);
				newTile.SetIsland(island, newTileVect);
				// Move tile to position in island
				newTile.transform.position += new Vector3(newTileVect.x * newTile.GetComponent<Collider2D>().bounds.size.x, newTileVect.y * newTile.GetComponent<Collider2D>().bounds.size.y, 0);
				newTile.SetSortingOrder();
				// Random chance to place entity
				if (Random.value < entitySpawnChance)
				{
					if (chosenTileInfo.uniqueTileEntities.Length > 0) {
						Entity chosenEntity = chosenTileInfo.uniqueTileEntities[Random.Range(0, chosenTileInfo.uniqueTileEntities.Length)].GetComponentsInChildren<Entity>()[0];
						GameObject entityObj = chosenEntity.gameObject;
						// Set this entity to be on the FloatingIsland layer while it's unhooked
						entityObj.layer = LayerMask.NameToLayer("FloatingIsland");
						if (entityObj.transform.parent != null) {
							entityObj = entityObj.transform.parent.gameObject;
							chosenEntity.transform.parent.gameObject.layer = LayerMask.NameToLayer("FloatingIsland");
						}
						Entity spawnedEntity = Instantiate(entityObj, newTile.transform).GetComponentsInChildren<Entity>()[0];
						newTile.PlaceEntity(spawnedEntity, newTileVect);
					}
				}
				island.tiles.Add(newTileVect, newTile);
			}
		}
	}

	private void PlaceNpcs(Island island)
	{
		int npcsToSpawn = (int)npcQuantity.GetRandom();
		int npcsSpawned = 0;
		List<Vector2> tileList = new List<Vector2>(island.tiles.Keys);
		while (npcsSpawned < npcsToSpawn && npcsSpawned < tileList.Count)
		{
			// Create NPC and place it onto a random tile
			Vector2 randTileVector = tileList[Random.Range(0, tileList.Count)];
			Tile chosenTile = island.tiles[randTileVector];
			// Don't spawn NPCs on tiles with entities
			if (!chosenTile.HasEntity())
			{
				GameObject chosenNpc = npcPrefabs[Random.Range(0, npcPrefabs.Length)];
				GameObject npc = Instantiate(chosenNpc, chosenTile.transform.position, Quaternion.identity, chosenTile.transform);
				ShiftSpriteToTile(npc.transform, chosenTile);
				// Flip animal half the time
				if (Random.value < 0.5f)
				{
					npc.GetComponent<NPCAnimator>().FlipSprite();
				}
				npc.GetComponent<Animal>().Sleep();
			}
			// Increment even if tile is unsuitable for NPC
			npcsSpawned++;
		}
	}

	// Shifts a transform up on its tile so its feet/bottom are on the center of the tile
	private void ShiftSpriteToTile(Transform t, Tile tile)
	{
		t.position += new Vector3(0, tile.gameObject.GetComponent<Renderer>().bounds.extents.y, 0);
	}

	/*
	// Randomly translate and flip decor
	public void RandomizeDecor(Tile tile)
	{
		Vector3 tileExtents = tile.GetComponent<Collider2D>().bounds.extents;
		Transform decor = tile.transform.Find("decor");
		if (decor != null) {
			Vector3 decorExtents = decor.GetComponent<Renderer>().bounds.extents;
			// Horizontally flip decor half the time
			if (Random.value < 0.5f) {
				decor.localScale *= -1;
			}
			// Randomly translate decor
			decor.position += new Vector3(Random.Range(0, tileExtents.x - decorExtents.x), Random.Range(0, tileExtents.y - decorExtents.y), 0);
		}
	}*/

	private Vector2 GetRandAdjacentVector(Vector2 vector) {
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
