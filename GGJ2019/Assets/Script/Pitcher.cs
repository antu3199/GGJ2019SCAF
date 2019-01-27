using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pitcher : MonoBehaviour {

    public float maxMagnitude;
    public float angularMagnitude;
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
                index = -1;
                break;
            }

            index = (index + 1) % ItemManager.Instance.inventory.itemSlots.Count;
            count++;
        }
        while(ItemManager.Instance.inventory.itemSlots[index].empty);
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
        } else {
            Item item = itemSlot.item;
            itemSlot.RemoveItems(1);
            GameObject itemProjectile = itemGen.GetOverworldItem(item);
            Vector3 launchPoint = transform.position;
            Vector2 chrDir = Character.DirToVector(player.direction);
            launchPoint.x += chrDir.x * 2;
            launchPoint.y += chrDir.y * 2;
            itemProjectile.transform.position = launchPoint;
            Vector2 throwVelocity = chrDir * currentMagnitude;
            itemProjectile.GetComponent<Rigidbody2D>().velocity = throwVelocity;
            itemProjectile.GetComponent<Rigidbody2D>().angularVelocity = Random.Range(-1 * angularMagnitude, angularMagnitude);
        }

        currentMagnitude = 0;
    }

    // COROUTINE
    
    IEnumerator Charge() {
        while(charging) {
            currentMagnitude += incrementor;
            currentMagnitude = currentMagnitude > maxMagnitude ? maxMagnitude : currentMagnitude;
            yield return new WaitForFixedUpdate();
        }
        yield return null;
    }


}
