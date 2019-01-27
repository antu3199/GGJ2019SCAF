using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class HarvestPrefab {
    public string key;
    public HarvestableEntity entity;
}

public class SoilEntity : Entity
{
    public string itemKey;
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

    public override void Interact(/*Player player,*/ EntityDirection dir = EntityDirection.NONE)
    {
        if (!hasPlanted)
        {
            HarvestPrefab prefab = vegetablePrefabs.Find((p) => p.key == itemKey);

            HarvestableEntity newItem = Instantiate<HarvestableEntity>(prefab.entity);
            newItem.parent = this.transform;
            newItem.transform.parent = this.transform;
            newItem.transform.position = this.transform.position;
            newItem.transform.localPosition = new Vector3(newItem.transform.localPosition.x, newItem.transform.localPosition.y, -0.1f);
            this.harvest = newItem;
            this.hasPlanted = true;
        
        } else
        {
           this.harvest.Interact();
        }

    }

} 
