using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour {

	// Default values
	public float defaultShakeDuration;
	public float defaultShakeMagnitude;
	public float defaultDampingSpeed;

	private float currShakeDuration;
	private float currShakeMagnitude;
	private float currDampingSpeed;

	private Camera cam;
	private Vector3 initialPosition;

	void Awake () {
		cam = Camera.main;
	}

	void Start()
	{
		initialPosition = transform.localPosition;
	}

	void Update()
	{
		if (currShakeDuration > 0) {
			transform.localPosition = initialPosition + Random.insideUnitSphere * currShakeMagnitude;
			currShakeDuration -= Time.deltaTime * currDampingSpeed;
		} else {
			transform.localPosition = initialPosition;
		}
	}

	public void TriggerShake()
	{
		currShakeDuration = defaultShakeDuration;
		currShakeMagnitude = defaultShakeMagnitude;
		currDampingSpeed = defaultDampingSpeed;
	}

	public void TriggerShake(float duration=0, float magnitude=0, float dampingSpeed=0)
	{
		if (duration == 0) {
			currShakeDuration = defaultShakeDuration;
		} else {
			currShakeDuration = duration;
		}
		if (magnitude == 0) {
			currShakeMagnitude = defaultShakeMagnitude;
		} else {
			currShakeMagnitude = magnitude;
		}
		if (dampingSpeed == 0) {
			currDampingSpeed = defaultDampingSpeed;
		} else {
			currDampingSpeed = dampingSpeed;
		}
	}
}
