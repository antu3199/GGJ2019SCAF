using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EntityDirection
{
    NONE = -1,
    UP = 0,             //Up is default for entity direction
    DOWN = 1,
    LEFT = 2,
    RIGHT = 3
};

public class Entity : MonoBehaviour {
    //public Vector2 pivotPosition; //TODO: If we assume all pivots are on 0,0 (bottom-left) we may not need this
    //public List<Tile> tileEntities; //TODO: Try to avoid this reference as possible
    public Vector2[] occupancy;
    public virtual bool interactable { get; set; }
    public EntityDirection direction;

    public void SetSortingOrder(int order) {
        GetComponent<SpriteRenderer>().sortingOrder = order;
    }

    public Vector2[] GetDirectionalOccupancy() {
        List<Vector2> rotated = new List<Vector2>();
        if(direction == EntityDirection.UP) {
            return occupancy;
        }
        else if(direction == EntityDirection.DOWN) {
            foreach(Vector2 pos in occupancy) {
                rotated.Add(new Vector2(pos.x * -1, pos.y * -1));
            }
        }
        else if(direction == EntityDirection.RIGHT) {
            foreach(Vector2 pos in occupancy) {
                rotated.Add(new Vector2(pos.y, pos.x * -1));
            }   
        }
        else if(direction == EntityDirection.LEFT) {
            foreach(Vector2 pos in occupancy) {
                rotated.Add(new Vector2(pos.y * -1, pos.x));
            }   
        }
        else {
            return occupancy; //TODO: This should not happen
        }

        return rotated.ToArray();
    }

    public virtual void Interact(/*Player player,*/ EntityDirection dir = EntityDirection.NONE) { }

    void Start() { }
    void Update() { }
}
