using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public int id;
    public string description;
    public string itemName;
    public int count;
    private bool isPicked;
    private Inventory inventory;

    private Dictionary<string, string> notChangableInformation;

    protected void Start() {
        isPicked = false;

        notChangableInformation = new Dictionary<string, string> {
            {"id", this.id.ToString()},
            {"itemName", this.itemName},
            {"description", this.description},
        };
    }

    private void OnTriggerEnter2D(Collider2D collision) {

        if(collision.CompareTag("Player") && !isPicked) {
            Inventory inventory = collision.GetComponent<Inventory>();
            inventory.GetItem(this.gameObject);
            this.gameObject.SetActive(false);
            isPicked = true;
        }
    }

    public abstract void UseItem();
}
