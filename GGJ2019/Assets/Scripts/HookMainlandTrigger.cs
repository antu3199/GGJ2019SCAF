using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookMainlandTrigger : MonoBehaviour {
	public Hook hook;

	void OnTriggerEnter2D(Collider2D col) {
		Tile tile = col.gameObject.GetComponent<Tile>();
		if(tile && tile.islandRef.island.type == IslandType.GridLocked) {
			hook.dropPoint = tile;
		}
	}
}