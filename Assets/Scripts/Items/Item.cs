using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public int id;
    private bool isPicked;
    private Inventory inventory;

    protected void Start()
    {
        isPicked = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && !isPicked)
        {
            //HotBar hotBar = collision.GetComponent<HotBar>();
            // hotBar.GetItem(this.gameObject);
            Inventory inventory = collision.GetComponent<Inventory>();
            //inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<Inventory>();
            inventory.GetItem(this.gameObject);
            this.gameObject.SetActive(false);
            isPicked = true;
        }
    }

    public abstract void UseItem();
}
