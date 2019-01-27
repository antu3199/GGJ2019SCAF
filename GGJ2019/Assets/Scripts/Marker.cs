using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marker : MonoBehaviour {
    // Object class for indicating which tile the player will be interacting with

    public GameObject indicator;
    public Tile selectedTile;
    public Collider2D selectedTileCollider;
    public Collider2D[] tiles;
    Collider2D marker;
    public ContactFilter2D tileFilter;

	// Use this for initialization
	void Start () {
        // markers must be child of some gameobject
		if (transform.parent == null)
        {
            Destroy(gameObject);
        }
        marker = GetComponent<Collider2D>();
        tiles = new Collider2D[4];

        // populate tiles 
        // IMPORTANT! - this assumes player spawns somewhere with tiles in front of them
        //circle.OverlapCollider(tileFilter, tiles);
        //selectedTileCollider = tiles[0];
        //selectedTile = selectedTileCollider.GetComponent<Tile>();
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Entered " + collision.GetComponent<Tile>().coordinate.ToString());
        for (int i = 0; i < tiles.Length; i++)
        {
            tiles[i] = null;
        }
        marker.OverlapCollider(tileFilter, tiles);
        selectedTileCollider = selectTile();
        selectedTile = selectedTileCollider.GetComponent<Tile>();
        indicator.transform.position = selectedTile.transform.position;
        Debug.Log("Selected " + selectedTile.coordinate.ToString());
    }

    /*  IF selectedTile and tiles are already populated, then we only
     *  need to run this again when selectedTile exits the CircleCollider.
     */
    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Exited " + collision.GetComponent<Tile>().coordinate.ToString());
        Debug.Log(collision.GetComponent<Tile>().coordinate.ToString() + " == " + (collision == selectedTileCollider));
        if (collision == selectedTileCollider)
        {
            for (int i = 0; i < tiles.Length; i++)
            {
                tiles[i] = null;
            }
            marker.OverlapCollider(tileFilter, tiles);
            selectedTileCollider = selectTile();
            selectedTile = selectedTileCollider.GetComponent<Tile>();
            indicator.transform.position = selectedTile.transform.position;
            Debug.Log("Selected " + selectedTile.coordinate.ToString());
        }
    }

    // Given rotation in euler angles of parent, return appropriate square to look at
    private Collider2D selectTile ()
    {
        Collider2D selected = null;
        float selectedAngleDiff = float.MaxValue;
        foreach (Collider2D t in tiles)
        {
            if (t)
                Debug.Log(t.GetComponent<Tile>().coordinate.ToString() + " " + transform.parent.rotation.eulerAngles.z + " vs " + Vector2.Angle(Vector2.up, transform.parent.position - t.transform.position));
            if (t && Mathf.Abs(transform.parent.rotation.eulerAngles.z - Vector2.Angle(Vector2.up, transform.parent.position - t.transform.position)) < selectedAngleDiff)
            {
                selected = t;
            }
        }
        return selected;
    }
}
