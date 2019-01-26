using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour {

    // Character status
    public string chrName;
    public float maxHunger;
    public float maxHealth;

    public float currentHunger;
    public float currentHealth;

	// Use this for initialization
	public virtual void Start () {
        currentHealth = maxHealth;
        currentHunger = maxHunger;
	}
	
	// Update is called once per frame
	public virtual void Update () {
		// TODO reduce hunger regularly
        // TODO reduce health regularly if hunger is 0?
        // Can do in separate method so can be overridden
	}
}
