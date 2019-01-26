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
        if (!(marker = GetComponentInChildren<Marker>())){
            Debug.LogError("Player must have a marker attached!");
        }
	}
	
	// Update is called once per frame
	public override void Update () {
        // Controls
        r.velocity = moveSpeed * new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        if (r.velocity != Vector2.zero)
        {
            float rotation = Vector2.SignedAngle(Vector2.up, r.velocity);
            Vector3 angles = transform.eulerAngles;
            angles.z = rotation;
            transform.eulerAngles = angles;
        }
    }
}
