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


public abstract class TileObject
{
    public Sprite sprite;
    public Entity tileEntity;
    public Vector2 position;

    public TileObject (Entity entity)
    {
        this.SetEntity(entity);
    }

    public void SetEntity(Entity entity)
    {
        this.tileEntity = entity;
    }

}


public class Entity : MonoBehaviour {
    public Vector2 pivotPosition;
    public List<TileObject> tileEntities;
    public bool interactable;
    public EntityDirection direction;
    public virtual void Interact(EntityDirection dir = EntityDirection.NONE) { }

    void Start() { }
    void Update() { }
}
