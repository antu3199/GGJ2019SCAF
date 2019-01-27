using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseTool : MonoBehaviour {

    public Hook tool;
	
    Player player;

    void Start() {
        player = GetComponent<Player>();
    }

	// Update is called once per frame
	void Update () {
		if (Input.GetAxisRaw("Fire1") != 0 && !tool.isFired)
        {
            float dRotation = transform.rotation.eulerAngles.z * Mathf.Deg2Rad;
            Vector2 rotationVector = new Vector2(-Mathf.Sin(dRotation), Mathf.Cos(dRotation));
            tool.transform.position = player.transform.position;
            tool.gameObject.SetActive(true);
            tool.Fire(rotationVector);
            
            StartCoroutine(HideToolWhenComplete());
        }
	}

    // COROUTINE

    IEnumerator HideToolWhenComplete() {
        while(tool.isFired) {
            yield return new WaitForFixedUpdate();
        }

        tool.gameObject.SetActive(false);
    }
}
