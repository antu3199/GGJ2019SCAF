using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EntityDirection
{
    NONE,
    UP,
    DOWN,
    LEFT,
    RIGHT
};

public class Entity : MonoBehaviour {
    //public Vector2 pivotPosition; //TODO: If we assume all pivots are on 0,0 (bottom-left) we may not need this
    //public List<Tile> tileEntities; //TODO: Try to avoid this reference as possible
    public Vector2[] occupancy;
    public bool interactable;
    public EntityDirection direction;

    public void SetSortingOrder(int order) {
        GetComponent<SpriteRenderer>().sortingOrder = order;
    }

    public virtual void Interact(/*Player player,*/ EntityDirection dir = EntityDirection.NONE) { }

    void Start() { }
    void Update() { }
}
