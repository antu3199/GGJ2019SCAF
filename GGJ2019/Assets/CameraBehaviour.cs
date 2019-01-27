using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour {

    public Player player;
    Camera cam;

	// Use this for initialization
	void Start () {
        cam = GetComponent<Camera>();
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

        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
	}
}
