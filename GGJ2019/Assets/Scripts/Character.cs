using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour {

    public enum Direction
    {
        NORTH = 0,
        NORTHEAST = 1,
        EAST = 2,
        SOUTHEAST = 3,
        SOUTH = 4,
        SOUTHWEST = 5,
        WEST = 6,
        NORTHWEST = 7,
        NUM_DIRECTIONS = 8
    }

    public static Vector2 DirToVector(Direction d)
    {
        switch (d)
        {
            case Direction.NORTH:
                return Vector2.up;
            case Direction.NORTHEAST:
                return new Vector2(1, 1).normalized;
            case Direction.EAST:
                return Vector2.right;
            case Direction.SOUTHEAST:
                return new Vector2(1, -1).normalized;
            case Direction.SOUTH:
                return Vector2.down;
            case Direction.SOUTHWEST:
                return new Vector2(-1, -1).normalized;
            case Direction.WEST:
                return Vector2.left;
            case Direction.NORTHWEST:
                return new Vector2(-1, 1).normalized;
            default:
                return Vector2.up;
        }
    }

    // Character status
    public string chrName;
    public float maxHunger;
    public float maxHealth;

    public float currentHunger;
    public float currentHealth;

    public Direction direction;

	// Use this for initialization
	public virtual void Start () {
        currentHealth = maxHealth;
        currentHunger = maxHunger;
        direction = Direction.NORTH;
	}
	
	// Update is called once per frame
	public virtual void Update () {
		// TODO reduce hunger regularly
        // TODO reduce health regularly if hunger is 0?
        // Can do in separate method so can be overridden
	}

}
