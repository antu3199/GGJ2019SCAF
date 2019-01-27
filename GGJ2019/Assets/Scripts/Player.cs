using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character {

    public float moveSpeed;
    public CameraBehaviour cam;

    Rigidbody2D r;
    Marker marker;
    Pitcher arm;
    Animator anim;
    
	// Use this for initialization
	public override void Start () {
        base.Start();
        r = GetComponent<Rigidbody2D>();
        if (!(marker = GetComponentInChildren<Marker>())){
            Debug.LogError("Player must have a marker attached!");
        }
        arm = GetComponent<Pitcher>();
        anim = GetComponent<Animator>();

		StartCoroutine(StartHungerDrain());
	}

	// Update is called once per frame
	public override void Update () {
        if(!isDead) {
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
                if (marker.selectedTile && marker.selectedTile.entityRef.entity && marker.selectedTile.entityRef.entity.interactable == true)
                {
                    marker.selectedTile.entityRef.entity.Interact();
                }
            }

            if (Input.GetKeyDown(KeyCode.X)) {
                arm.InitCharge();
            }

            if(Input.GetKeyUp(KeyCode.X)) {
                arm.Fire();
            }

            if(Input.GetKeyDown(KeyCode.E)) {
                arm.IterateToNextValidItemSlot();
            }
        }
    }

    public override void Die() {
        base.Die();
        cam.enabled = false;
        OverworldItemGenerator itemGen = GetComponent<OverworldItemGenerator>();

        foreach(PlayerItemSlot itemSlot in ItemManager.Instance.inventory.itemSlots) {
            Item item = itemSlot.item;
            for (int i = 0; i < itemSlot.quantity; i++) {
                itemSlot.RemoveItems(1);
                GameObject itemProjectile = itemGen.GetOverworldItem(item);
                itemProjectile.transform.position = transform.position;
                Rigidbody2D rb = itemProjectile.GetComponent<Rigidbody2D>();
                rb.AddForce(new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(0.5f, 1f), 0) * 600f);
                rb.freezeRotation = false;
                rb.gravityScale = 0.5f;
                rb.angularDrag = 0.5f;
                rb.AddTorque(100f);
            }
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
