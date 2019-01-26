using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marker : MonoBehaviour {
    // Object class for indicating which tile the player will be interacting with

    // public Tile selectedTile;
    // public Tile[4] tiles;
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
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Collider2D[] overlaps = new Collider2D[4];
        circle.OverlapCollider(tileFilter, overlaps);
    
        // TODO Add in first tile if selectedTile is empty 
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Collider2D[] overlaps = new Collider2D[4];
        circle.OverlapCollider(tileFilter, overlaps);

        // TODO set new tile as selectedTile if old selectedTile is out
        /* if (collision.GetComponent<Tile>() == selectedTile){
        *      // choose one from existing list
        *      selectedTile = overlaps[0].GetComponent<Tile>();
        *  }
        */
    }
}
