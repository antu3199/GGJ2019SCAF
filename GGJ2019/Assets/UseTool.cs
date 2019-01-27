using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseTool : MonoBehaviour {

    public Hook tool;
	
	// Update is called once per frame
	void Update () {
		if (Input.GetAxisRaw("Fire1") != 0 && !tool.isFired)
        {
            float dRotation = transform.rotation.eulerAngles.z * Mathf.Deg2Rad;
            Vector2 rotationVector = new Vector2(Mathf.Cos(dRotation), Mathf.Sin(dRotation));
            tool.Fire(rotationVector);
        }
	}
}
