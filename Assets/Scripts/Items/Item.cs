using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public int id;
    private bool isPicked;

    protected void Start()
    {
        isPicked = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && !isPicked)
        {
            Inventory inventory = collision.GetComponent<Inventory>();
            inventory.GetItem(this.gameObject);
            this.gameObject.SetActive(false);
            isPicked = true;
        }
    }
}
