using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType
{
    GHOST = 0,
    OWNED = 1,
    UNOWNED = 2
}

public class EntityRef {
    public Entity entity;
    public Vector2 location;

    public EntityRef(Entity entity, Vector2 offset) {
        this.entity = entity;
        this.location = offset;
    }
}

public class IslandRef {
    public Island island;
    public Vector2 location; 

    public IslandRef(Island island, Vector2 offset) {
        this.island = island;
        this.location = offset;    
    }
}

public class Tile : MonoBehaviour {
    public TileType tileType;
    public Vector2 coordinate;          //Relative to Game
    public EntityRef entityRef;
    public IslandRef islandRef;

    public int GetSortingOrder() {
        return (int)(-coordinate.y * 100);
    }

    public void SetSortingOrder() {
		GetComponent<SpriteRenderer>().sortingOrder = GetSortingOrder();
    }

    public bool HasEntity() {
        return entityRef != null;
    }

    public void PlaceEntity(Entity entity, Vector2 offset) {
        entityRef = new EntityRef(entity, offset);
    }

    public void SetIsland(Island island, Vector2 offset) {
        this.islandRef = new IslandRef(island, offset);
    }
}
