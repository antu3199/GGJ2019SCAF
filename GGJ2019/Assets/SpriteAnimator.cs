using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class FrameState
{
    public Sprite sprite;
    public float timeBeforeTransition;
}

public class SpriteAnimator : MonoBehaviour {

    public int animState = 0;
    public List<FrameState> states;
    public SpriteRenderer spriteRenderer;
    public bool loop = false;
    public float timePassed { get; set; }

    public bool finished { get; set; }

    public bool paused { get; set; }
    private float nextTransitionTime;

    void Start()
    {
        finished = false;
        paused = false;
        this.Reset();
    }

    float GetNextTransitionTime()
    {
        return timePassed + states[animState].timeBeforeTransition;
    }

    void Update()
    {
        if (!paused && !finished && animState < states.Count)
        {
            timePassed += Time.deltaTime;
            if (timePassed >= nextTransitionTime)
            {

                GetNextFrame();


                if (animState == states.Count)
                {
                    if (!loop)
                    {
                        finished = true;
                    } else
                    {
                        animState = -1;
                        GetNextFrame();
                    }
                }
            }
        }

    }

    public void Reset()
    {
        animState = 0;
        finished = false;
        spriteRenderer.sprite = states[0].sprite;
        nextTransitionTime = GetNextTransitionTime();
    }

    private void GetNextFrame()
    {
        animState++;
        if (animState < states.Count && animState >= 0)
        {
            spriteRenderer.sprite = states[animState].sprite;
            nextTransitionTime = GetNextTransitionTime();
        }
    }
}
