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
	}

	void Start() {
		rb = GetComponent<Rigidbody2D>();
		UpdateChildTiles();
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
			Vector2 newLocation = MigratePiece(pair.Key, pair.Value, origin, player);
			targetTile.transform.position = coordinateToPosition(newLocation);
			//TODO: Some animation
		}
		Destroy(other.gameObject);
	}

	public IEnumerator BeginTimeout(float lifetime) {
		yield return new WaitForSeconds(lifetime);
		Destroy(gameObject);
	}

	/// PRIVATE

	private Vector2 MigratePiece(Vector2 tileLoc, Tile tile, Vector2 origin, Player player) {
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
		tile.transform.SetParent(gameObject.transform);
		return selection;
	}

	private Vector3 coordinateToPosition(Vector2 coord) {
		return transform.position + (Vector3)(coord * map.tileSize);
	}

	private void UpdateChildTiles() {
		foreach(KeyValuePair<Vector2, Tile> pair in tiles) {
			pair.Value.SetIsland(this, pair.Key);
		}
	}
}