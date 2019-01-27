﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldItem : MonoBehaviour {

    public Item item;
    public float duration;
    public Rigidbody2D rb;
    SpriteRenderer spriteRenderer;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetSprite();
        StartCoroutine(DestroyMe());
	}
	
	public void SetSprite()
    {
        if (item && spriteRenderer)
            spriteRenderer.sprite = item.sprite;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        int otherLayer = collision.otherCollider.gameObject.layer;
        if (LayerMask.LayerToName(otherLayer) == "Player")
        {
            // Pickup
        }
    }

    IEnumerator DestroyMe() {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }

}
