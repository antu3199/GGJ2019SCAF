using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour {

    [SerializeField] private Text selectedItemTitleText;
    [SerializeField] private Text selectedItemDescriptionText;
    [SerializeField] private JButton useButton;
    [SerializeField] private Image selectedBorder;
    [SerializeField] private Image selectedItemImage;

    public List<PlayerItemSlot> itemSlots;

    public GridLayoutGroup itemsGrid;
    public int selectedItemIndex = -1;
    private int maxStorage;
    public bool opened;

    public void Initialize()
    {
        maxStorage = itemsGrid.transform.childCount;
        itemSlots = new List<PlayerItemSlot>(itemsGrid.GetComponentsInChildren<PlayerItemSlot>());
        for (int i = 0; i < itemSlots.Count; i++)
        {
            itemSlots[i].Initialize(i);
        }

        this.selectedItemIndex = -1;
        this.UpdateSelected();


        //For testing
        for (int i = 5; i < ItemManager.Instance.itemData.Count; i++)
        {
            AddItem(ItemManager.Instance.itemData[i].key, 1);
        }

    }

    public void ToggleInventory()
    {
        this.selectedItemIndex = -1;
        this.UpdateSelected();
        this.gameObject.SetActive(!this.gameObject.activeSelf);
    }


    public void UpdateSelected()
    {

        if (this.selectedItemIndex != -1)
        {
            PlayerItemSlot itemSlot = this.itemSlots[this.selectedItemIndex];
            if (itemSlot.empty || itemSlot.item == null)
            {
                this.selectedItemIndex = -1;
            }
        }

        bool selected = this.selectedItemIndex != -1;

        if (!selected)
        {
            this.selectedItemTitleText.text = "";
            this.selectedItemDescriptionText.text = "";
            this.selectedBorder.gameObject.SetActive(false);
            useButton.Interactable = false;
            this.selectedItemImage.gameObject.SetActive(false);
        } else
        {
            this.selectedBorder.gameObject.SetActive(true);
            PlayerItemSlot itemSlot = this.itemSlots[this.selectedItemIndex];
            this.selectedItemTitleText.text = itemSlot.itemModel.displayName;
            this.selectedItemDescriptionText.text = itemSlot.itemModel.description;
            useButton.SetAction(itemSlot.item.Use);
            selectedBorder.transform.SetParent(itemSlot.itemSlotImage.transform, false);
            useButton.Interactable = itemSlot.item.usable && selected;
            this.selectedItemImage.sprite = itemSlot.itemSlotImage.sprite;
            this.selectedItemImage.gameObject.SetActive(true);
        }

        UIManager.Instance.UpdateSelectedItem(UIManager.Instance.selectedIndex);
    }


    public bool AddItem(string itemKey, int quantity)
    {

        if (!isValidItem(itemKey)) return false;

        foreach (PlayerItemSlot itemSlot in itemSlots)
        {
            if (itemSlot.itemModel != null && itemSlot.itemModel.key == itemKey && itemSlot.empty == false && itemSlot.itemModel.collapsable == true)
            {
                itemSlot.AddItems(quantity);
                return true;
            }
        }

        foreach (PlayerItemSlot itemSlot in itemSlots)
       {
            if (itemSlot.empty)
            {
                itemSlot.SetItem(itemKey);
                itemSlot.AddItems(quantity);
                return true;
            }
       }

        
        .Log("Inventory is full!");

        return false;
    }

    public bool RemoveItem(string itemKey, int quantity = 1)
    {

        if (!isValidItem(itemKey) || !hasEnoughItems(itemKey, quantity)) return false;

        List<PlayerItemSlot> specificItemSlots = GetPlayerItemsSlots(itemKey);


        int collapableQuantity = quantity;
        int count = 0;
        foreach (PlayerItemSlot itemSlot in specificItemSlots)
        {

            count = itemSlot.quantity;
            itemSlot.RemoveItems(quantity);
            quantity -= count;
        }

        this.UpdateSelected();

        return true;
    }

    public bool hasEnoughItems(string itemKey, int quantity)
    {
        if (!isValidItem(itemKey)) return false;

        List<PlayerItemSlot> specificItemSlots = GetPlayerItemsSlots(itemKey);

        foreach (PlayerItemSlot itemSlot in specificItemSlots)
        {
            quantity -= itemSlot.quantity;
            if (quantity <= 0) return true;
        }


        return quantity <= 0;
    }

    public void SelectItem (int index)
    {
        this.selectedItemIndex = index;
        this.UpdateSelected();
    }

    private List<PlayerItemSlot> GetPlayerItemsSlots(string itemKey)
    {
        List<PlayerItemSlot> retItemSlots = new List<PlayerItemSlot>();

        if (!isValidItem(itemKey)) return retItemSlots;

        foreach (PlayerItemSlot itemSlot in itemSlots)
        {
            if (itemSlot.itemModel != null && itemSlot.itemModel.key == itemKey && itemSlot.empty == false)
            {
                retItemSlots.Add(itemSlot);
            }
        }

        return retItemSlots;
    }

    private bool isValidItem(string itemKey)
    {
        ItemModel model = ItemManager.Instance.GetItemModel(itemKey);

        if (model == null)
        {
            Debug.LogError("Invalid item!");
            return false;
        }

        return true;
    }



}
