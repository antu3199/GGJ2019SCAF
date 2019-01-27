using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AnimalState
{
	SLEEPING = 0,
	ACTIVE = 1
}

public class Animal : Character
{
	public AnimalState state;
	public RandomValue moveDelay;		// Time (range) to wait until moving again.
	public RandomValue moveTime;		// Duration (range) for which to move.
	public RandomValue moveForce;
	public RandomValue wakeTransitionTime;	// Time between waking up and moving around.
	Rigidbody2D rb;
	NPCAnimator npcAnimator;

	void Awake()
	{
		state = AnimalState.ACTIVE;
		npcAnimator = GetComponent<NPCAnimator>();
	}

	public override void Start()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	public override void Update()
	{
		npcAnimator.SetFloat("xVelocity", Mathf.Abs(rb.velocity.x));
	}

	public void Sleep()
	{
		state = AnimalState.SLEEPING;
		npcAnimator.Trigger("Sleep");
	}

	public IEnumerator WakeUp()
	{
		state = AnimalState.ACTIVE;
		npcAnimator.Trigger("Wake");
		yield return new WaitForSeconds(wakeTransitionTime.GetRandom());
		StartCoroutine(Wander());
	}

	private void Move()
	{
		float xForce = moveForce.GetRandom();
		if (Random.value < 0.5f) {
			xForce *= -1;
		}
		float yForce = moveForce.GetRandom();
		if (Random.value < 0.5f) {
			yForce *= -1;
		}
		if (npcAnimator.flipped && xForce < 0) {
			npcAnimator.FlipSprite();
		} else if (!npcAnimator.flipped && xForce > 0) {
			npcAnimator.FlipSprite();
		}
		Debug.Log(new Vector2(xForce, yForce));
		rb.AddForce(new Vector2(xForce, yForce));
	}

	IEnumerator Wander()
	{
		while (true)
		{
			Move();
			yield return new WaitForSeconds(moveTime.GetRandom());
			rb.velocity = Vector2.zero;
			yield return new WaitForSeconds(moveDelay.GetRandom());
			if (state == AnimalState.SLEEPING)
			{
				break;
			}
		}
	}
}
