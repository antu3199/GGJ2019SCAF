using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum CameraState {
	FOLLOWING,
	SHAKING
}

public class CameraBehaviour : MonoBehaviour {

    public Player player;
    Camera cam;
	CameraState cameraState;

	// Shake variables

	// Default values
	public float defaultShakeDuration;
	public float defaultShakeMagnitude;
	public float defaultDampingSpeed;
	private float currShakeDuration;
	private float currShakeMagnitude;
	private float currDampingSpeed;

	// Use this for initialization
	void Start () {
        cam = GetComponent<Camera>();
		cameraState = CameraState.FOLLOWING;
	}
	
	// Update is called once per frame
	void Update () {
        // Rely on Map to assign player to camera
		if (!player)
        {
			return;
        }
		float proposedCamSize = cam.orthographicSize + Input.GetAxisRaw("Mouse ScrollWheel") * 5;
        if (12 <= proposedCamSize && proposedCamSize <= 50)
        {
            cam.orthographicSize = proposedCamSize;
        }

		if (cameraState == CameraState.FOLLOWING)
		{
			transform.position = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
		}
	}

	IEnumerator Shake()
	{
		cameraState = CameraState.SHAKING;
		while (true)
		{
			if (currShakeDuration > 0) {
				transform.localPosition = player.transform.position + new Vector3(Random.value, Random.value, 0) * currShakeMagnitude;
				currShakeDuration -= Time.deltaTime * currDampingSpeed;
			} else {
				transform.localPosition = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
				break;
			}
			yield return null;
		}
		cameraState = CameraState.FOLLOWING;
	}

	public void TriggerShake()
	{
		currShakeDuration = defaultShakeDuration;
		currShakeMagnitude = defaultShakeMagnitude;
		currDampingSpeed = defaultDampingSpeed;
        StartCoroutine(Shake());
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
        StartCoroutine(Shake());
	}
}
