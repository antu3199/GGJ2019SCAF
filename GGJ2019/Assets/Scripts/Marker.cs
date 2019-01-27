using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marker : MonoBehaviour {
    // Object class for indicating which tile the player will be interacting with

    public Tile selectedTile;



    private void OnTriggerEnter2D(Collider2D collision)
    {

        Tile tile = collision.gameObject.GetComponent<Tile>();
        if (tile && tile.entityRef.entity)
        {
            this.selectedTile = tile;
            tile.entityRef.entity.OnEnterTile();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Tile tile = collision.gameObject.GetComponent<Tile>();
        if (tile && tile.entityRef.entity)
        {
            this.selectedTile = tile;
            tile.entityRef.entity.OnStayTile();
        }
    }

    /*  IF selectedTile and tiles are already populated, then we only
     *  need to run this again when selectedTile exits the CircleCollider.
     */
    private void OnTriggerExit2D(Collider2D collision)
    {
        Tile tile = collision.gameObject.GetComponent<Tile>();
        if (tile && tile.entityRef.entity)
        {
            this.selectedTile = tile;
            tile.entityRef.entity.OnExitTile();
        }
    }

}
