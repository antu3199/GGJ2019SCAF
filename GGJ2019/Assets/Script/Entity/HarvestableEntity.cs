using System.Collections.Generic;
using UnityEngine;

public class HarvestableEntity : Entity
{
    public string itemKey;
    public int harvestQuantity = 1;
    [SerializeField] private SpriteAnimator animator;

    public override bool interactable
    {
        get
        {
            return true;
        }

        set
        {
            base.interactable = value;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            this.Interact();
        }
    }

    public override void Interact(/*Player player,*/ EntityDirection dir = EntityDirection.NONE)
    {
        ItemManager.Instance.inventory.AddItem(this.itemKey, harvestQuantity);
        animator.Reset();
    }

} 
