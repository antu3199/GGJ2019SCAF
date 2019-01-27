using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAnimator : MonoBehaviour {

	public Animator animator;
	public bool flipped;

	void Awake () {
		animator = GetComponent<Animator>();
	}

	/*public void SetBool(string param, bool val) {
		animator.SetBool(param, val);
	}*/

	public void Trigger(string param) {
		animator.SetTrigger(param);
	}

	public void SetFloat(string param, float val) {
		animator.SetFloat(param, val);
	}

	public void FlipSprite() {
		Vector3 flipTransform = transform.localScale;
		flipTransform.x *= -1;
		transform.localScale = flipTransform;
		flipped = !flipped;
	}
}
