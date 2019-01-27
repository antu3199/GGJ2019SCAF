using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerItem : Item
{
    public override bool usable
    {
        get
        {
            return false;
        }

        set
        {
            base.usable = value;
        }
    }
}