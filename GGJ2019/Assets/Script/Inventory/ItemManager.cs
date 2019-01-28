using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class ItemFactory
{
    public static Dictionary<string, ItemModel> itemDictionary = new Dictionary<string, ItemModel>();
    private static string gameDataProjectFilePath = "/StreamingAssets/items.json";

    public static void Initialize()
    {
        

        string filePath = Application.dataPath + gameDataProjectFilePath;
        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            ItemsList itemsList = JsonUtility.FromJson<ItemsList>(dataAsJson);
            foreach (ItemModel itemModel in itemsList.items)
            {
                itemDictionary.Add(itemModel.key, itemModel);
            }
        }
        else
        {
            Debug.LogError("Error finding items");
        }
    }
}

[Serializable]
public class ItemsList
{
    public List<ItemModel> items;
}

[Serializable]
public class ItemModel
{
    public int index; // All items should have an index that uniquely identify it.
    public string key;
    public string displayName;
    public string description;
    public bool usable;
    public bool collapsable; //If you have two of the item, will it take one or two slots?
}


public class ItemManager : Singleton<ItemManager> {

    public List<Item> itemData { get; set; }
    public PlayerInventory inventory;
    
    void Awake()
    {
        ItemFactory.Initialize();
        itemData = new List<Item>(this.GetComponentsInChildren<Item>());
        inventory.Initialize();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            this.inventory.ToggleInventory();
        }
    }

	public ItemModel GetItemModel(string key)
    {
        if (key == null) return null;

        if (ItemFactory.itemDictionary.ContainsKey(key))
        {
            return ItemFactory.itemDictionary[key];
        }

        return null;
    }

    public ItemModel GetItemModel(int index)
    {
        foreach (ItemModel model in ItemFactory.itemDictionary.Values)
        {
            if (model.index == index)
            {
                return model;
            }
        }

        return null;
    }

    public Item GetItemData(string key)
    {
        return itemData.Find(item => item.key == key);
    }
}
