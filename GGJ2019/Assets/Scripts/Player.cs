using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character {

    public float moveSpeed;

    Rigidbody2D r;
    Marker marker;
    Animator anim;
    
	// Use this for initialization
	public override void Start () {
        base.Start();
        r = GetComponent<Rigidbody2D>();
        if (!(marker = GetComponentInChildren<Marker>())){
            Debug.LogError("Player must have a marker attached!");
        }
        anim = GetComponent<Animator>();

		StartCoroutine(StartHungerDrain());
	}

	// Update is called once per frame
	public override void Update () {
        // Controls
        r.velocity = moveSpeed * new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        if (r.velocity != Vector2.zero)
        {
            float rotation = Vector2.SignedAngle(r.velocity, Vector2.up) + 360;
            direction = (Direction)(Mathf.Round(rotation / 45) % 8);
        }
        anim.SetInteger("direction", animDirection());
        anim.SetBool("isMoving", r.velocity != Vector2.zero);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (marker.selectedTile && marker.selectedTile.entityRef.entity)
            {
                marker.selectedTile.entityRef.entity.Interact();
            }
		}

		if (currentHealth <= 0)
		{
			//TODO: game over
		}
	}

	int animDirection()
    {
        return (int)direction / 2 * 2;
    }

	IEnumerator StartHungerDrain()
	{
		while (true)
		{
			if (currentHunger > 0)
			{
				// Reduce hunger regularly
				IncreaseHunger(-hungerLossPerTick);
				yield return new WaitForSeconds(hungerTickDuration);
			}
			else {
				// Reduce health regularly if hunger is 0
				IncreaseHealth(-healthLossPerTick);
				yield return new WaitForSeconds(starvingTickDuration);
			}
			if (currentHealth <= 0)
			{
				break;
			}
		}
	}
}
