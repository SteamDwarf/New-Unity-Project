using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    [SerializeField] protected int id;
    [SerializeField] protected string description;
    [SerializeField] protected string itemName;
    [SerializeField] protected int count;
    protected bool isPicked;
    public ItemUseType useType;
    private Inventory inventory;

    public GameObject thisObjectPrefab;

    protected void Start() {
        isPicked = false;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision) {

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

/*     public abstract Item Clone(); */
    public abstract void UseItem();
    public abstract void SetInformation(Dictionary<string, object> information);
    public abstract Dictionary<string, object> GetItemInformation();
}
