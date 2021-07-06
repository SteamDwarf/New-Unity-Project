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

    protected void Start() {
        isPicked = false;
    }

    private void OnTriggerEnter2D(Collider2D collision) {

        if(isPicked) {
            return;
        }

        if(collision.CompareTag("Player")) {
            Inventory inventory = collision.GetComponent<Inventory>();
            inventory.GetItem(this.gameObject);
            this.gameObject.SetActive(false);
            isPicked = true;
        }
    }

    private IEnumerator DropItemCorutine() {
        isPicked = true;
        yield return new WaitForSeconds(5f);
        isPicked = false;
    }

    public void DropItem() {
        StartCoroutine(DropItemCorutine());
    }
    public abstract void UseItem();
    public abstract void SetInformation(Dictionary<string, object> information);
    public abstract Dictionary<string, object> GetItemInformation();
}
