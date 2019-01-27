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
			bool exclusionResolved = false;
			float rand;
			if (excludedRange.min < range.min && range.min < excludedRange.max) {
				range.min = excludedRange.max;
				exclusionResolved = true;
			}
			if (excludedRange.min < range.max && range.max < excludedRange.max) {
				range.max = excludedRange.min;
				exclusionResolved = true;
			}
			if (exclusionResolved) {
				rand = Random.Range(range.min, range.max);
			} else {
				float excludedInterval = (excludedRange.max - excludedRange.min);
				rand = Random.Range(range.min, range.max - excludedInterval);
				if (rand >= excludedRange.min) {
					rand += excludedInterval;
				}
			}
			return rand;
		}
		else {
			return Random.Range(range.min, range.max);
		}
	}
}
