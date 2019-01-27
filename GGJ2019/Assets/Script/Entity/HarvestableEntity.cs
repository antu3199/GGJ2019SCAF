using System.Collections.Generic;
using UnityEngine;

public class HarvestableEntity : Entity
{
    public string itemKey;
    public int harvestQuantity = 1;

    [SerializeField] private SpriteAnimator animator;

    public int minSeedsDropped = 1;
    public int maxSeedsDropped = 2;
    public string seedKey;

    void Start()
    {
        this.animator.SetCallBack(this.UpdateCallback);
    }

    public override bool interactable
    {
        get
        {
            return animator.animState == animator.states.Count;
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

    public override void OnStayTile()
    {
        base.OnStayTile();
        actionImage.gameObject.SetActive(this.interactable);
    }

    public override void OnEnterTile() {
        base.OnEnterTile();
    }

    public override void OnExitTile() {
        base.OnExitTile();
        this.actionImage.gameObject.SetActive(false);
    }

    public override void Interact(/*Player player,*/ EntityDirection dir = EntityDirection.NONE)
    {
        if (!this.interactable) return;

        ItemManager.Instance.inventory.AddItem(this.itemKey, harvestQuantity);
        if (ItemManager.Instance.GetItemData(this.seedKey) != null)
        {
            ItemManager.Instance.inventory.AddItem(this.seedKey, Random.Range(minSeedsDropped, maxSeedsDropped));
        }

        animator.Reset();

        this.actionImage.gameObject.SetActive(this.interactable);

    }

    private void UpdateCallback()
    {
        if (this.inRange)
            actionImage.gameObject.SetActive(this.interactable);
    }

} 
