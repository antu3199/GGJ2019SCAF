using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerItemSlot : MonoBehaviour
{

    public Image itemSlotImage;
    [SerializeField] private Text quantityText;
    [SerializeField] private JButton itemButton;

    public int index;

    public ItemModel itemModel = null; //Left public for testing
    public Item item { get; set; }

    public int quantity = 0;
    public bool empty = true;



    void Start()
    {
        this.EnableItemUI(false);
        this.itemButton.SetAction(this.OnClick);
    }

    public void Initialize(int index)
    {
        this.index = index;
    }

    public void SetItem(string key)
    {
        this.itemModel = ItemManager.Instance.GetItemModel(key);
        this.item = ItemManager.Instance.GetItemData(key);
        this.itemSlotImage.sprite = this.item.sprite;

        this.EnableItemUI(true);
    }

    public void AddItems(int quantity)
    {
        if (this.itemModel == null || quantity <= 0) return;

        this.quantity += quantity;
        if (this.quantity > 0) empty = false;
        this.UpdateQuantity();
    }

    public void RemoveItems(int quantity)
    {
        if (this.itemModel == null || quantity <= 0) return;

        this.quantity -= quantity;
        if (this.quantity <= 0)
        {
            this.ResetItemSlot();
        }
        this.UpdateQuantity();
    }

    private void UpdateQuantity()
    {
        this.quantityText.text = quantity.ToString();
    }

    private void ResetItemSlot()
    {
        itemModel = null;
        quantity = 0;
        empty = true;
        this.UpdateQuantity();
        this.EnableItemUI(false);
    }

    private void EnableItemUI(bool enable)
    {
        itemSlotImage.gameObject.SetActive(enable);
        quantityText.gameObject.SetActive(enable);
        this.itemButton.Interactable = enable;
    }

    private void OnClick()
    {
        if (item == null) return;
        ItemManager.Instance.inventory.SelectItem(this.index);
    }
}