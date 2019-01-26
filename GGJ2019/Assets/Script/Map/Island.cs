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

	void Start() {
		tiles = new Dictionary<Vector2, Tile>();
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

	/// PRIVATE

	private void UpdateChildTiles() {
		foreach(KeyValuePair<Vector2, Tile> pair in tiles) {
			pair.Value.SetIsland(this, pair.Key);
		}
	}
}