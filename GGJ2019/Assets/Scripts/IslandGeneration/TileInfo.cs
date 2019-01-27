using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileInfo {

	public GameObject tilePrefab;

	public GameObject[] uniqueTileEntities;		// Entity prefabs that correspond to this tile prefab.
}
