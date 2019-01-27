using System.Collections.Generic;
using UnityEngine;

public class FlowerGarden : Entity
{
    public override bool interactable
    {
        get
        {
            return false;
        }

        set
        {
            base.interactable = value;
        }
    }

    public override void Interact(/*Player player,*/ EntityDirection dir = EntityDirection.NONE)
    {
        //Do something
    }

} 
