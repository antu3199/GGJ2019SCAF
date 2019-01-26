using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marker : MonoBehaviour {
    // Object class for indicating which tile the player will be interacting with

    public Tile selectedTile;
    public Collider2D selectedTileCollider;
    public Collider2D[] tiles;
    CircleCollider2D circle;
    public ContactFilter2D tileFilter;

	// Use this for initialization
	void Start () {
        // markers must be child of some gameobject
		if (transform.parent == null)
        {
            Destroy(gameObject);
        }
        circle = GetComponent<CircleCollider2D>();
        tiles = new Collider2D[4];

        // populate tiles 
        // IMPORTANT! - this assumes player spawns somewhere with tiles in front of them
        circle.OverlapCollider(tileFilter, tiles);
        selectedTileCollider = tiles[0];
        selectedTile = selectedTileCollider.GetComponent<Tile>();
	}

    /*  IF selectedTile and tiles are already populated, then we only
     *  need to run this again when selectedTile exits the CircleCollider.
     */
    private void OnTriggerExit2D(Collider2D collision)
    {   if (collision == selectedTileCollider)
        {
            circle.OverlapCollider(tileFilter, tiles);
            selectedTileCollider = tiles[0];
            selectedTile = selectedTileCollider.GetComponent<Tile>();
        }
    }
}
