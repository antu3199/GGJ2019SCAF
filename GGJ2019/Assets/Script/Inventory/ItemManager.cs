using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;


public class ItemFactory : MonoBehaviour
{
    public static Dictionary<string, ItemModel> itemDictionary = new Dictionary<string, ItemModel>();
	private static string itemDataURL = "https://raw.githubusercontent.com/nansonzheng/GGJ2019SCAF/master/GGJ2019/Assets/StreamingAssets/items.json";

	public static ItemFactory instance;

	public static IEnumerator Initialize(ItemManager itemManager)
    {
		string filePath = Path.Combine(Application.streamingAssetsPath, "items.json");
		// If file path is URL
		if (filePath.Contains("://") || filePath.Contains(":///"))
		{
			UnityWebRequest www = UnityWebRequest.Get(filePath);
			yield return www.SendWebRequest();

			if (www.isNetworkError || www.isHttpError)
			{
				Debug.LogError("Error fetching items from URL");
			}
			else
			{
				ProcessJson(www.downloadHandler.text);
			}
		}
		else if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
			ProcessJson(dataAsJson);
        }
        else
        {
            Debug.LogError("Error finding items");
        }
    }

	private static void ProcessJson(string data)
	{
		ItemsList itemsList = JsonUtility.FromJson<ItemsList>(data);
		foreach (ItemModel itemModel in itemsList.items)
		{
			itemDictionary.Add(itemModel.key, itemModel);
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
		StartCoroutine(Initialize());
    }

	IEnumerator Initialize()
	{
		yield return StartCoroutine(ItemFactory.Initialize(this));
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
