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

	public float hungerTickDuration;	// Time between hunger ticks.
	public float hungerLossPerTick;
	public float starvingTickDuration;	// Time between taking damage when starving.
	public float healthLossPerTick;

	public Direction direction;

    protected bool isDead;

	// Use this for initialization
	public virtual void Start () {
        currentHealth = maxHealth;
        currentHunger = maxHunger;
        isDead = false;
        direction = Direction.NORTH;
        StartCoroutine(WatchHealth());
	}
	
	// Update is called once per frame
	public virtual void Update () {

	}

	public void IncreaseHunger(float value)
	{
		currentHunger += value;
		currentHunger = Mathf.Clamp(currentHunger, 0, maxHunger);
        UIManager.Instance.UpdateHunger(currentHunger, maxHunger);
    }

	public void IncreaseHealth(float value)
	{
		currentHealth += value;
		currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
	}

	protected virtual void Die()
	{
		isDead = true;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        foreach(Collider2D col in GetComponentsInChildren<Collider2D>()) {
            col.enabled = false;
        }

        rb.freezeRotation = false;
        rb.AddForce(new Vector3(0, 1, 0) * 700f);
        rb.gravityScale = 0.5f;
        rb.angularDrag = 2;
        rb.AddTorque(4000f);
		Destroy(gameObject, 10f);
	}

    // COROUTINE

    IEnumerator WatchHealth() {
        while(true) {
            if(currentHunger  <= 0 || currentHealth <= 0) {
                Debug.Log("idie");
                Die();
                break;
            }

            yield return new WaitForFixedUpdate();
        }

        yield return null;
    }
}
