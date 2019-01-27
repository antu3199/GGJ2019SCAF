using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldItemGenerator : MonoBehaviour {

    public GameObject template;

	public GameObject GetOverworldItem(Item item)
    {
        GameObject copy = Instantiate<GameObject>(template);
        OverworldItem owi = copy.GetComponent<OverworldItem>();
        owi.item = item;
        owi.SetSprite();

        return copy;
    }
}
