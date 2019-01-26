using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour {
	public Player player;

	public float magnitude;
	public float rewindMagnitude;
	public float WaitForRewind;

	public bool isFired {get; private set;}
	public bool collision {get; private set;}
	
	Rigidbody2D rb;
	Tile cargo;
	

	void Start() {
		rb = GetComponent<Rigidbody2D>();
		isFired = false;
		collision = false;
	}

	public void Fire(Vector2 dir) {
		if(!isFired) {
			isFired = true;
			rb.velocity = dir * magnitude;
			StartCoroutine(Rewind());	
		}
		
	}

	private bool IsCloseTo(float num, float num2) {
		return (num - num2 <= 0.01f);
	}

	void OnTriggerEnter2D(Collider2D col) {
		Tile tile = Collider2D.gameObject.GetComponent<Tile>();
		if(tile.islandRef.island.type == IslandType.FreeForm) {
			cargo = tile;
		}
	}

	// COROUTINES

	private IEnumerator WaitForEvent() {
		float startTime = Time.time;
		while(true) {
			if(Time.time - startTime >= WaitForRewind) {
				StartCoroutine(Rewind());
				break;
			}

			if(collision) {
				StartCoroutine(Rewind());
				break;
			}
			yield return new WaitForEndOfFrame();
		}
		
		yield return null;
	}

	private IEnumerator Rewind() {
		float startTime = Time.time;
		float totalDist = Vector2.Distance(this.transform.position, player.transform.position);
		rb.velocity = Vector2.zero;
		//TODO: Turn off Rewind hitbox??
		while(true) {
			float distCovered = (Time.time - startTime) * rewindMagnitude;
			float fracJourney = distCovered / totalDist;
			transform.position = Vector3.Lerp(this.transform.position, player.transform.position, fracJourney);
			if(cargo) {
				cargo.transform.position = Vector3.Lerp(cargo.transform.position, player.transform.position, fracJourney);
			}

			yield return new WaitForEndOfFrame();

			if(IsCloseTo(fracJourney, 1.0f)) {
				isFired = false;
				yield return null;
			}
		}
	}
}