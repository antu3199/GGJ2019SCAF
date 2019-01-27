using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour {
	public Player player;

	public float magnitude;
	public float rewindMagnitude;
	public float WaitForRewind;

	public bool isFired {get; private set;}
	public bool collision {get; set;}
	
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
		gameObject.SetActive(false);
	}

	public void Fire(Vector2 dir) {
		if(!isFired) {
			isFired = true;
			EnableCargoTrigger();
			rb.velocity = dir * magnitude;
            Vector3 rotation = transform.eulerAngles;
            rotation.z = Vector2.SignedAngle(Vector2.up, dir);
            transform.eulerAngles = rotation;
            StartCoroutine(WaitForEvent());	
		}
		
	}

	private bool IsCloseTo(float num, float num2) {
		return (Mathf.Abs(num - num2) <= 0.01f);
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
		collision = false;
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
		Vector3 startPos = this.transform.position;
		Vector3 cargoPos = cargo ? cargo.islandRef.island.transform.position : this.transform.position;
		rb.velocity = Vector2.zero;

		while(true) {
			float distCovered = (Time.time - startTime) * rewindMagnitude;
			float fracJourney = distCovered / totalDist;
			transform.position = Vector3.Lerp(startPos, player.transform.position, fracJourney);
			if(cargo) {
				cargo.islandRef.island.transform.position = Vector3.Lerp(cargoPos, player.transform.position, fracJourney);
			}

			yield return new WaitForEndOfFrame();

			if(dropPoint && cargo) {
				dropPoint.islandRef.island.MergeIsland(cargo.islandRef.island, cargo, dropPoint.islandRef.location, player);
				dropPoint = null;
				cargo = null;
			}

			if(IsCloseTo(fracJourney, 1.0f) || fracJourney > 1.0f) {
				ResetHook();
				break;
			}
		}

		yield return null;
	}
}