using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public Sprite sprite;
    private SpriteRenderer spriteRend;
    private string spriteName;
    private Inventory inventory;

    protected void Start()
    {
        spriteRend = GetComponent<SpriteRenderer>();
        sprite = spriteRend.sprite;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log(spriteRend);
            inventory = collision.GetComponent<Inventory>();
            inventory.GetItem(this);
            this.gameObject.SetActive(false);
        }
    }

}
