using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChickenDrops : DropSetter {

	void Start() {
		drops = new List<Item>();
		drops.Add(ItemManager.Instance.GetItemData("egg-raw"));
	}
}