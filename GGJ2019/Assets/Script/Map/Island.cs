using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum IslandType {
	FreeForm,
	GridLocked,
}

public class Island : MonoBehaviour {
	public Vector2 coordinate;
	public Dictionary<Vector2, Tile> tiles;
	public IslandType type;
	public Rigidbody2D rb;
	public Map map;

	void Awake() {
		tiles = new Dictionary<Vector2, Tile>();
		InitializeChildren();
	}

	void Start() {
		rb = GetComponent<Rigidbody2D>();
	}

	public bool IsEntityPlaceable(Entity entity, Vector2 pivot) {
		foreach(Vector2 occupant in entity.GetDirectionalOccupancy()) {
			if(!tiles.ContainsKey(occupant + pivot)) {
				return false;
			}
		}
		return true;
	}

	public void PlaceEntity(Entity entity, Vector2 pivot) {
		int sortingOrder = int.MinValue;
		foreach(Vector2 occupant in entity.GetDirectionalOccupancy()) {
			tiles[occupant + pivot].PlaceEntity(entity, occupant);
			if (tiles[occupant + pivot].GetSortingOrder() < sortingOrder) {
				sortingOrder = tiles[occupant + pivot].GetSortingOrder();
			}
		}

		entity.SetSortingOrder(sortingOrder);
	}

	public void MergeIsland(Island other, Tile targetTile, Vector2 targetLocation, Player player) {
		Vector2 origin = targetLocation - targetTile.islandRef.location;
		foreach(KeyValuePair<Vector2, Tile> pair in other.tiles) {
			Vector2 newLocation = MigratePiece(other, pair.Key, pair.Value, origin, player);
			WakeUpEntities();
			//TODO: Some animation
		}
		Camera.main.GetComponent<CameraBehaviour>().TriggerShake();
	}

	public void RemoveTile(Vector2 coord) {
		if(tiles.ContainsKey(coord)) {
			tiles.Remove(coord);
		}

		//Destroy self if no tiles
		if(tiles.Count == 0) {
			StopAllCoroutines();
			BeginTimeout(3f);
		}
	}

	public IEnumerator BeginTimeout(float lifetime) {
		yield return new WaitForSeconds(lifetime);
		Destroy(gameObject);
	}

	/// PRIVATE

	private void WakeUpEntities() {
		foreach(Animal animal in GetComponentsInChildren<Animal>()) {
			animal.transform.parent = null;
			animal.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
			StartCoroutine(animal.WakeUp());
		}
	}

	private Vector2 MigratePiece(Island other, Vector2 tileLoc, Tile tile, Vector2 origin, Player player) {
		List<Vector2> acceptableTargets = new List<Vector2>();

		int ring = 0;
		int offsetx = 0;
		int offsety = ring - offsetx;
		Vector2 targetPlacement = tileLoc + origin + new Vector2(offsetx, offsety);

		//Fancy check for closest tile(s) which are acceptable
		while(true) {
			if(!tiles.ContainsKey(targetPlacement)) {
				acceptableTargets.Add(targetPlacement);
			}

			if(offsety == 0) {
				if(acceptableTargets.Count > 0) {
					break;
				}
				ring++;
				offsetx = 0;
				offsety = ring;
			} else {
				offsetx++;
				offsety = ring - offsetx;	
			}
			targetPlacement = tileLoc + origin + new Vector2(offsetx, offsety);
		}

		float score = float.MinValue;
		Vector2 selection = new Vector2(0, 0);
		Vector2 hookVector = tile.transform.position - player.transform.position;

		//Pick the best scoring target
		foreach(Vector2 target in acceptableTargets) {
			Vector2 targetVector = coordinateToPosition(target) - player.transform.position;
			float result = Vector2.Dot(hookVector.normalized, targetVector.normalized);
			if(result > score) {
				selection = target;
				score = result;
			}
		}

		//Migrate the Tile
		map.PlaceTile(tile, (int)(selection.x + coordinate.x), (int)(selection.y + coordinate.y));
		tile.islandRef.island = this;
		tile.islandRef.location = selection;
		tile.gameObject.layer = gameObject.layer;
		tile.transform.SetParent(gameObject.transform);
		tiles.Add(selection, tile);
		other.RemoveTile(selection);

		return selection;
	}

	private void InitializeChildren() {
		foreach(Tile tile in GetComponentsInChildren<Tile>()) {
			tile.islandRef.island = this;
			tiles.Add(tile.islandRef.location, tile);
		}
	}

	private Vector3 coordinateToPosition(Vector2 coord) {
		return (Vector3)(coordinate * map.tileSize) + (Vector3)(coord * map.tileSize);
	}

	private void UpdateChildTiles() {
		foreach(KeyValuePair<Vector2, Tile> pair in tiles) {
			pair.Value.SetIsland(this, pair.Key);
		}
	}
}