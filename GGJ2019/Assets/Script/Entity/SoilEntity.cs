using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class HarvestPrefab {
    public string key;
    public HarvestableEntity entity;
}

public class SoilEntity : Entity
{

    [SerializeField] private HarvestableEntity prePlantedEntity;

    public string itemKey;
    public string itemSeedKey;
    public List<HarvestPrefab> vegetablePrefabs;
    public BoxCollider2D boxCollider;

    private bool hasPlanted = false;

    private HarvestableEntity harvest;

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
        if (Input.GetKeyDown(KeyCode.Space) && this.inRange)
        {
            this.Interact();
        }
    }

    public override void OnStayTile()
    {
        base.OnStayTile();
        if (hasPlanted)
        {
            this.harvest.OnStayTile();
        }
    }

    public override void OnEnterTile() {
        base.OnEnterTile();
        if (hasPlanted)
        {
            this.harvest.OnEnterTile();
        }
    }

    public override void OnExitTile() {
        base.OnExitTile();
        if (hasPlanted)
        {
            this.harvest.OnExitTile();
        }
    }

    public void PlantNow()
    {
        hasPlanted = true;
        this.harvest = prePlantedEntity;
    }

    public override void Interact(/*Player player,*/ EntityDirection dir = EntityDirection.NONE)
    {
        if (!hasPlanted)
        {

            if (ItemManager.Instance.inventory.itemSlots[UIManager.Instance.selectedIndex].empty)
            {
                return;
            }

            string selectedKey = ItemManager.Instance.inventory.itemSlots[UIManager.Instance.selectedIndex].itemModel.key;
            if (selectedKey.Length < 6)
            {
                return;
            }

            string seedText = selectedKey.Substring(selectedKey.Length - 6);

            string foodKey = selectedKey.Substring(0, selectedKey.Length - 6); //remove seeds

            if (ItemManager.Instance.GetItemData(foodKey) == null || ItemManager.Instance.GetItemData(selectedKey) == null || seedText != "-seeds" || !ItemManager.Instance.inventory.hasEnoughItems(selectedKey, 1))
            {
                return;
            }

            this.itemKey = foodKey;
            this.itemSeedKey = selectedKey;

            HarvestPrefab prefab = vegetablePrefabs.Find((p) => p.key == itemKey);

            HarvestableEntity newItem = Instantiate<HarvestableEntity>(prefab.entity);
            newItem.parent = this.transform;
            newItem.transform.parent = this.transform;
            newItem.transform.position = this.transform.position;
            newItem.transform.localPosition = new Vector3(newItem.transform.localPosition.x, newItem.transform.localPosition.y, -0.1f);
            this.harvest = newItem;
            this.hasPlanted = true;
            ItemManager.Instance.inventory.RemoveItem(itemSeedKey, 1);


        }
        else
        {
           this.harvest.Interact();
        }

    }

} 
