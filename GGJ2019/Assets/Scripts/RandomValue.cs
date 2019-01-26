using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RandomValue {

	public Range range;
	Range excludedRange;

	public RandomValue(float min, float max)
	{
		range = new Range(min, max);
	}

	public void SetExcludedRange(Range range)
	{
		excludedRange = range;
	}

	public float GetRandom()
	{
		if (excludedRange != null)
		{
			float excludedInterval = (excludedRange.max - excludedRange.min);
			float rand = Random.Range(range.min, range.max - excludedInterval);
			if (rand >= excludedRange.min)
			{
				rand += excludedInterval;
			}
			return rand;
		}
		else {
			return Random.Range(range.min, range.max);
		}
	}
}
