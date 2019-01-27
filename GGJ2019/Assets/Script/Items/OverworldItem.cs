using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldItem : MonoBehaviour {

    public Item item;
    public float duration;
    public Rigidbody2D rb;
    SpriteRenderer spriteRenderer;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetSprite();
        StartCoroutine(DestroyMe());
	}
	
	public void SetSprite()
    {
        if (item && spriteRenderer)
            spriteRenderer.sprite = item.sprite;
    }

    IEnumerator DestroyMe() {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }

     private void OnTriggerEnter2D(Collider2D collision)
    {
        int otherLayer = collision.gameObject.layer;
        string layerName = LayerMask.LayerToName(otherLayer);
        if (layerName == "Player")
        {
            ItemManager.Instance.inventory.AddItem(item.key, 1);
            Destroy(gameObject);
        }
        else
        {
            Character victim = collision.gameObject.GetComponent<Character>();
            if (victim) {
                victim.IncreaseHealth(-100); //TODO: change value
                Debug.Log("Smashed " + victim.chrName + "!");
                Destroy(gameObject);
            }
            
        }

    }

}
