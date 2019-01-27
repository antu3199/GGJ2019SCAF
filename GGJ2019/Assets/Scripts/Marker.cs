using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marker : MonoBehaviour {
    // Object class for indicating which tile the player will be interacting with

    public GameObject visualObject;
    public Tile selectedTile;

    GameObject visual;
    Collider2D col;

    private void Awake()
    {
        visual = Instantiate(visualObject);
        visual.SetActive(false);
    }

    private void Start()
    {
        col = GetComponent<Collider2D>();
    }

    private void Update()
    {
        col.offset = 2 * Character.DirToVector(transform.parent.GetComponent<Player>().direction);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Tile tile = collision.gameObject.GetComponent<Tile>();
        if (tile)
        {
            this.selectedTile = tile;
            SetVisualPosition(selectedTile.transform.position);
            if (selectedTile.entityRef.entity)
            {
                selectedTile.entityRef.entity.OnEnterTile();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (selectedTile && selectedTile.entityRef.entity)
        {
            selectedTile.entityRef.entity.OnStayTile();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Tile tile = collision.gameObject.GetComponent<Tile>();
        if (tile && tile.entityRef.entity)
        {
            tile.entityRef.entity.OnExitTile();
        }
    }

    void SetVisualPosition(Vector3 position)
    {
        visual.SetActive(true);
        visual.transform.position = new Vector3(position.x, position.y, visual.transform.position.z);
    }
}
