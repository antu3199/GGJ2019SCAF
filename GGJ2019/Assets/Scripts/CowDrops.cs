using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CowDrops : DropSetter {

	void Start() {
		drops = new List<Item>();
		drops.Add(ItemManager.Instance.GetItemData("steak-raw"));
	}
}