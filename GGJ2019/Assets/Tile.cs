using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType
{
    GHOST = 0,
    OWNED = 1,
    UNOWNED = 2
}

public class Tile : MonoBehaviour {

    public TileType tileType;
    public Vector2 coordinate;

    public int GetSortingOrder() {
        return (int)(-coordinate.y * 100);
    }

    public void SetSortingOrder() {
		GetComponent<SpriteRenderer>().sortingOrder = GetSortingOrder();
    }
}
