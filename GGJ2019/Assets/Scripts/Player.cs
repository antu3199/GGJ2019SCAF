using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character {

    public float moveSpeed;

    Rigidbody2D r;
    Marker marker;
    
	// Use this for initialization
	public override void Start () {
        base.Start();
        r = GetComponent<Rigidbody2D>();
        if (!(marker = GetComponent<Marker>())){
            Debug.LogError("Player must have a marker attached!");
        }
	}
	
	// Update is called once per frame
	public override void Update () {
        // Controls
        r.velocity = moveSpeed * new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
	}
}
