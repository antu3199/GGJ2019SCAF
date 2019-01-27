using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decor : MonoBehaviour {

	public Sprite[] decorSprites;

	public void RandomizeDecor()
	{
		if (decorSprites.Length > 0) {
			GetComponent<SpriteRenderer>().sprite = decorSprites[Random.Range(0, decorSprites.Length)];
		} else {
			Debug.Log("Warning! Decor has no sprites.");
		}
	}
}
