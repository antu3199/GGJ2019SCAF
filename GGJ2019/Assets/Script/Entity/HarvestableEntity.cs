using System.Collections.Generic;
using UnityEngine;

public class HarvestableEntity : Entity
{
    public string itemKey;
    public int harvestQuantity = 1;
    public BoxCollider2D boxCollider;

    [SerializeField] private SpriteAnimator animator;

    public int minSeedsDropped = 1;
    public int maxSeedsDropped = 2;
    public string seedKey;


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
        if (ItemManager.Instance.GetItemData(this.seedKey) != null)
        {
            ItemManager.Instance.inventory.AddItem(this.seedKey, Random.Range(minSeedsDropped, maxSeedsDropped));
        }
        animator.Reset();
    }

} 
