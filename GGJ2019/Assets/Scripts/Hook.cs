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
	
	public Tile cargo {get; set;}
	public Tile dropPoint {get; set;}

	Rigidbody2D rb;
	
	HookIslandTrigger islandTrigger;
	HookMainlandTrigger mainlandTrigger;

	void Start() {
		rb = GetComponent<Rigidbody2D>();
		islandTrigger = GetComponentInChildren<HookIslandTrigger>();
		mainlandTrigger = GetComponentInChildren<HookMainlandTrigger>();
		isFired = false;
		collision = false;

		islandTrigger.hook = this;
		mainlandTrigger.hook = this;
	}

	public void Fire(Vector2 dir) {
		if(!isFired) {
			isFired = true;
			EnableCargoTrigger();
			rb.velocity = dir * magnitude;
			StartCoroutine(Rewind());	
		}
		
	}

	private bool IsCloseTo(float num, float num2) {
		return (num - num2 <= 0.01f);
	}

	private void EnableCargoTrigger() {
		islandTrigger.gameObject.SetActive(true);
		mainlandTrigger.gameObject.SetActive(false);
	}

	private void EnableDropPointTrigger() {
		islandTrigger.gameObject.SetActive(false);
		mainlandTrigger.gameObject.SetActive(true);
	}

	private void DisableAllTrigger() {
		islandTrigger.gameObject.SetActive(false);
		mainlandTrigger.gameObject.SetActive(false);
	}

	private void ResetHook() {
		isFired = false;
		cargo = null;
		dropPoint = null;
		DisableAllTrigger();
	}

	// COROUTINES

	private IEnumerator WaitForEvent() {
		float startTime = Time.time;
		while(true) {
			if(Time.time - startTime >= WaitForRewind) {
				DisableAllTrigger();
				StartCoroutine(Rewind());
				break;
			}

			if(collision) {
				EnableDropPointTrigger();
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

		while(true) {
			float distCovered = (Time.time - startTime) * rewindMagnitude;
			float fracJourney = distCovered / totalDist;
			transform.position = Vector3.Lerp(this.transform.position, player.transform.position, fracJourney);
			if(cargo) {
				cargo.transform.position = Vector3.Lerp(cargo.transform.position, player.transform.position, fracJourney);
			}

			yield return new WaitForEndOfFrame();

			if(dropPoint) {
				dropPoint.islandRef.island.MergeIsland(cargo.islandRef.island, cargo, dropPoint.islandRef.location, player);
				dropPoint = null;
			}

			if(IsCloseTo(fracJourney, 1.0f)) {
				ResetHook();
				yield return null;
			}
		}
	}
}