using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableItem :Item {
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
        ItemManager.Instance.inventory.RemoveItem(this.key, 1);
    }
}
