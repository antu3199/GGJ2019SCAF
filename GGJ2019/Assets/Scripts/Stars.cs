using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stars : MonoBehaviour {

	public float flickerFreq;
	public List<Sprite> sprites;

	SpriteRenderer sr;
	public int curSprite;

	void Start() {
		curSprite = 0;
		sr = GetComponent<SpriteRenderer>();
		StartCoroutine(Animate());
	}

	// COROUTINE

	IEnumerator Animate() {
		while(true)
		{
			sr.sprite = sprites[curSprite];
			yield return new WaitForSeconds(flickerFreq);
			curSprite = (curSprite + 1) % sprites.Count;	
		}
	}
}