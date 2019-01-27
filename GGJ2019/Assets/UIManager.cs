using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{

    public Image selectedItemImage;
    public Text selectedItemText;

    public Slider hungerSlider;

    public int selectedIndex;

    void Start()
    {
        UpdateSelectedItem(0);
    }

    public void UpdateHunger (float currentHunger, float maxHunger)
    {
        float fract = currentHunger / maxHunger;
        hungerSlider.value = fract;
    }

    public void UpdateSelectedItem(int index)
    {

        this.selectedIndex = index;
        if (index != -1 && !ItemManager.Instance.inventory.itemSlots[index].empty)
        {
            this.selectedItemImage.sprite = ItemManager.Instance.inventory.itemSlots[index].itemSlotImage.sprite;
            this.selectedItemText.text = ItemManager.Instance.inventory.itemSlots[index].quantity.ToString();
            this.selectedItemImage.gameObject.SetActive(true);
            this.selectedItemText.gameObject.SetActive(true);
        } else
        {
            this.selectedItemImage.gameObject.SetActive(false);
            this.selectedItemText.gameObject.SetActive(false);
        }
    }

	public void InitializeItemImage()
    {

    }
}
