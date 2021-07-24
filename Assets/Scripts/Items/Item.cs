using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    [SerializeField] protected int id;
    [SerializeField] protected AudioClip pickupItemAudio;
    [SerializeField] protected int count;

    protected bool isPicked;
    public ItemUseType useType;
    private Inventory inventory;

    public GameObject thisObjectPrefab;

    protected virtual void OnTriggerEnter2D(Collider2D collision) {

        if(isPicked) {
            return;
        }

        if(collision.CompareTag("Player")) {
            Inventory inventory = collision.GetComponent<Inventory>();
            inventory.GetItem(this.gameObject);
            this.gameObject.SetActive(false);
            isPicked = true;

            if(collision.GetComponent<AudioPlayer>() != null) {
                collision.GetComponent<AudioPlayer>().PlayOneShot(pickupItemAudio);
            }
        }
    }

    protected IEnumerator DropItemCorutine() {
        isPicked = true;
        yield return new WaitForSeconds(2f);
        isPicked = false;
    }

    public void DropItem() {
        StartCoroutine(DropItemCorutine());
    }

    public int GetId() {
        return id;
    }
    public int GetCount() {
        return count;
    }
    public void SetCount(int count) {
        this.count = count;
    }

    public abstract void UseItem();
}
