using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandSpawner : MonoBehaviour {

	private Camera cam;
	public GameObject homeIsland;		// Reference to player's island.
	public IslandGenerator islandGenerator;
	public float homeBuffer;			// The closest distance an island can pass by the home island.
	public RandomValue spawnCooldown;
	public RandomValue islandVelocity;

	private void Start()
	{
		cam = Camera.main;
		//TODO: move this call to another script
		StartSpawning();
	}

	public void StartSpawning()
	{
		if (islandGenerator.tilePrefabs.Length > 0) {
			StartCoroutine(SpawnIslands());
		}
		else {
			Debug.LogError("IslandSpawner error: No island prefabs to choose from.");
		}
	}

	IEnumerator SpawnIslands()
	{
		while (true) {
			float halfCamHeight = cam.orthographicSize;
			float halfCamWidth = halfCamHeight * cam.aspect;

			// Get generated island object
			GameObject island = islandGenerator.GenerateIsland();

			// Randomly choose whether this island is vertically-moving or horizontally-moving
			if (Random.value < 0.5f) {
				// Vertically-moving island
				RandomValue spawnXRange = new RandomValue(cam.transform.position.x - halfCamWidth, cam.transform.position.x + halfCamWidth);
				spawnXRange.SetExcludedRange(new Range(GetBounds(homeIsland).min.x - homeBuffer - GetBounds(island).extents.x, GetBounds(homeIsland).max.x + homeBuffer + GetBounds(island).extents.x));
				float newIslandX = spawnXRange.GetRandom();
				// Randomly choose whether this island spawns from top or bottom
				if (Random.value < 0.5f) {
					// Island spawns from top and moves down
					float newIslandY = cam.transform.position.y + halfCamHeight + GetBounds(island).extents.y;
					Vector3 newIslandPosition = new Vector3(newIslandX, newIslandY, island.transform.position.z);
					// Move island to randomly generated position
					island.transform.position = newIslandPosition;
					Debug.Log("spawning at " + newIslandPosition);

					island.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -islandVelocity.GetRandom());
				} else {
					// Island spawns from bottom and moves up
					float newIslandY = cam.transform.position.y - halfCamHeight - GetBounds(island).extents.y;
					Vector3 newIslandPosition = new Vector3(newIslandX, newIslandY, island.transform.position.z);
					Debug.Log("spawning at " + newIslandPosition);

					island.transform.position = newIslandPosition;
					island.GetComponent<Rigidbody2D>().velocity = new Vector2(0, islandVelocity.GetRandom());
				}
			} else {
				// Horizontally-moving island
				RandomValue spawnYRange = new RandomValue(cam.transform.position.y - halfCamHeight, cam.transform.position.y + halfCamHeight);
				spawnYRange.SetExcludedRange(new Range(GetBounds(homeIsland).min.y - homeBuffer - GetBounds(island).extents.y, GetBounds(homeIsland).max.y + homeBuffer + GetBounds(island).extents.y));
				float newIslandY = spawnYRange.GetRandom();
				// Randomly choose whether this island spawns from left or right
				if (Random.value < 0.5f)
				{
					// Island spawns from left and moves right
					float newIslandX = cam.transform.position.x + halfCamWidth + GetBounds(island).extents.x;
					Vector3 newIslandPosition = new Vector3(newIslandX, newIslandY, island.transform.position.z);
					island.transform.position = newIslandPosition;
					island.GetComponent<Rigidbody2D>().velocity = new Vector2(-islandVelocity.GetRandom(), 0);
				}
				else
				{
					// Island spawns from right and moves left
					float newIslandX = cam.transform.position.x - halfCamWidth - GetBounds(island).extents.x;
					Vector3 newIslandPosition = new Vector3(newIslandX, newIslandY, island.transform.position.z);
					island.transform.position = newIslandPosition;
					island.GetComponent<Rigidbody2D>().velocity = new Vector2(islandVelocity.GetRandom(), 0);
				}
			}
			//TODO: island cleanup after they exit bounds
			// Generate wait time until next island spawning
			yield return new WaitForSeconds(spawnCooldown.GetRandom());
		}
	}

	private Bounds GetBounds(GameObject obj)
	{
		return obj.GetComponent<Collider2D>().bounds;
	}
}
