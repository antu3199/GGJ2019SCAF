using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookIslandTrigger : MonoBehaviour {
	public Hook hook;

	void OnTriggerEnter2D(Collider2D col) {
		Tile tile = col.gameObject.GetComponent<Tile>();
		if(tile && tile.islandRef.island.type == IslandType.FreeForm) {
			hook.cargo = tile;
			hook.collision = true;
		}
	}
}