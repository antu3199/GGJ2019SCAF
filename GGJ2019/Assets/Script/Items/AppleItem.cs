using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleItem :Item {
    public override bool usable
    {
        get
        {
            return true;
        }

        set
        {
            base.usable = value;
        }
    }

    public override void Use()
    {
        base.Use();
        Debug.Log("Apple used");
        ItemManager.Instance.inventory.RemoveItem(this.key, 1);
    }
}
