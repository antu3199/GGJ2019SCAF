using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PigDrops : DropSetter {

	void Start() {
		drops = new List<Item>();
		drops.Add(ItemManager.Instance.GetItemData("pork-raw"));
	}
}