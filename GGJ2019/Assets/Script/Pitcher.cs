using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pitcher : MonoBehaviour {

    public float maxMagnitude;
    public float incrementor;

    float currentMagnitude;
    Player player;
    int index;

    bool charging = false;

    OverworldItemGenerator itemGen;

    void Start() {
        currentMagnitude = 0;
        index = 0;
        player = GetComponent<Player>();

        itemGen = GetComponent<OverworldItemGenerator>();
    }

    public PlayerItemSlot GetCurrentItem() {
        if(index < 0 || index >= ItemManager.Instance.inventory.itemSlots.Count) {
            return null;
        }

        return ItemManager.Instance.inventory.itemSlots[index];
    }

    public void IterateToNextValidItemSlot() {
        int count = 0;
        index = index == -1 ? 0 : index;

        do {
            if(count >= ItemManager.Instance.inventory.itemSlots.Count) {
                Debug.Log("No items to throw!");
                index = -1;
                break;
            }

            index = (index + 1) % ItemManager.Instance.inventory.itemSlots.Count;
            count++;
        }
        while(ItemManager.Instance.inventory.itemSlots[index].empty);

        PlayerItemSlot itemSlot = GetCurrentItem();
        Debug.Log("Currently on " + itemSlot.item.key);
    }

    public void InitCharge() {
        if(!charging) {
            charging = true;
            StartCoroutine(Charge());
        }
    }

    public void Fire() {
        charging = false;
        // Spawn the overworld object and throw it
        PlayerItemSlot itemSlot = GetCurrentItem();
        if(itemSlot == null || itemSlot.empty) {
            Debug.Log("Throwing Nothing!");
        } else {
            Item item = itemSlot.item;
            itemSlot.RemoveItems(1);
            GameObject itemProjectile = itemGen.GetOverworldItem(item);
            itemProjectile.transform.position = transform.position;
            Debug.Log(player.chrName + ": Throwing " + item.key);
            Vector2 throwVelocity = Character.DirToVector(player.direction) * currentMagnitude;
            itemProjectile.GetComponent<Rigidbody2D>().velocity = throwVelocity;
        }

        currentMagnitude = 0;
    }

    // COROUTINE
    
    IEnumerator Charge() {
        while(charging) {
            currentMagnitude += incrementor;
            currentMagnitude = currentMagnitude > maxMagnitude ? maxMagnitude : currentMagnitude;
            Debug.Log(currentMagnitude + "/" + maxMagnitude);
            yield return new WaitForFixedUpdate();
        }
        yield return null;
    }


}
