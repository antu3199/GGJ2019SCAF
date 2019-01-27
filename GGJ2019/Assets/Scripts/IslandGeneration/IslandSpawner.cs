using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandSpawner : MonoBehaviour {

	private Camera cam;
	public GameObject homeIsland;			// Reference to player's island.
	public IslandGenerator islandGenerator;
	public float homeBuffer;				// The closest distance an island can pass by the home island.
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
		if (islandGenerator.tileInfo.Length > 0) {
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
				spawnXRange.SetExcludedRange(new Range(GetChildBounds(homeIsland).min.x - homeBuffer - GetChildBounds(island).extents.x, GetChildBounds(homeIsland).max.x + homeBuffer + GetChildBounds(island).extents.x));
				float newIslandX = spawnXRange.GetRandom();
				// Randomly choose whether this island spawns from top or bottom
				if (Random.value < 0.5f) {
					// Island spawns from top and moves down
					float newIslandY = cam.transform.position.y + halfCamHeight + GetChildBounds(island).extents.y;
					Vector3 newIslandPosition = new Vector3(newIslandX, newIslandY, island.transform.position.z);
					// Move island to randomly generated position
					island.transform.position = newIslandPosition;
					island.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -islandVelocity.GetRandom());
				} else {
					// Island spawns from bottom and moves up
					float newIslandY = cam.transform.position.y - halfCamHeight - GetChildBounds(island).extents.y;
					Vector3 newIslandPosition = new Vector3(newIslandX, newIslandY, island.transform.position.z);
					island.transform.position = newIslandPosition;
					island.GetComponent<Rigidbody2D>().velocity = new Vector2(0, islandVelocity.GetRandom());
				}
			} else {
				// Horizontally-moving island
				RandomValue spawnYRange = new RandomValue(cam.transform.position.y - halfCamHeight, cam.transform.position.y + halfCamHeight);
				spawnYRange.SetExcludedRange(new Range(GetChildBounds(homeIsland).min.y - homeBuffer - GetChildBounds(island).extents.y, GetChildBounds(homeIsland).max.y + homeBuffer + GetChildBounds(island).extents.y));
				float newIslandY = spawnYRange.GetRandom();
				// Randomly choose whether this island spawns from left or right
				if (Random.value < 0.5f)
				{
					// Island spawns from left and moves right
					float newIslandX = cam.transform.position.x + halfCamWidth + GetChildBounds(island).extents.x;
					Vector3 newIslandPosition = new Vector3(newIslandX, newIslandY, island.transform.position.z);
					island.transform.position = newIslandPosition;
					island.GetComponent<Rigidbody2D>().velocity = new Vector2(-islandVelocity.GetRandom(), 0);
				}
				else
				{
					// Island spawns from right and moves left
					float newIslandX = cam.transform.position.x - halfCamWidth - GetChildBounds(island).extents.x;
					Vector3 newIslandPosition = new Vector3(newIslandX, newIslandY, island.transform.position.z);
					island.transform.position = newIslandPosition;
					island.GetComponent<Rigidbody2D>().velocity = new Vector2(islandVelocity.GetRandom(), 0);
				}
			}
			// Generate wait time until next island spawning
			yield return new WaitForSeconds(spawnCooldown.GetRandom());
		}
	}

	private Bounds GetChildBounds(GameObject obj)
	{
		Bounds accumulatedBounds = new Bounds();
		Collider2D[] colliders = obj.GetComponentsInChildren<Collider2D>();
		foreach (Collider2D collider in colliders)
		{
			accumulatedBounds.Encapsulate(collider.GetComponent<Collider2D>().bounds);
		}
		return accumulatedBounds;
	}
}
