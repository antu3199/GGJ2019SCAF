using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTestController : MonoBehaviour {

    public PlayerInventory inventory;
    
    public void AddApple()
    {
        inventory.AddItem("apple", 1);
    }

    public void RemoveApple()
    {
        inventory.RemoveItem("apple", 1);
    }

    public void AddFlower()
    {
        inventory.AddItem("flower", 1);
    }

    public void RemoveFlower()
    {
        inventory.RemoveItem("flower", 1);
    }

    public void ClickItem(string key)
    {

    }
}
