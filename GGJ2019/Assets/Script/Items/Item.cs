using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
	public Player player;
    public string key;
    public Sprite sprite;
    public virtual bool usable { get; set; }

    public virtual void Use() { 
    	if(player != null) {
    		player.IncreaseHunger(2f);
    	}
    }
}