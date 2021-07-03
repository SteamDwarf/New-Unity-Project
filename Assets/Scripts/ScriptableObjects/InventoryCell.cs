using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class InventoryCell : ScriptableObject
{
    //public GameObject item;
    public Item item {get; [SerializeField] private set;} //Возможно заменить обратно на GameObject, чтобы потом Instatiate его
    public Sprite sprite {get; [SerializeField] private set;}
    public int countItem {get; [SerializeField] private set;}
    public int idItem {get; [SerializeField] private set;}
    public string description {get; [SerializeField] private set;}
    public string itemName {get; [SerializeField] private set;}

    public InventoryCell Clone() {
        InventoryCell copyCell = ScriptableObject.CreateInstance<InventoryCell>();

        copyCell.item = this.item;
        copyCell.sprite = this.sprite;
        copyCell.countItem = this.countItem;
        copyCell.idItem = this.idItem;
        copyCell.description = this.description;
        copyCell.itemName = this.itemName;

        return copyCell;
    }

    public void Clear() {
        item = null;
        sprite = null;
        countItem = 0;
        idItem = 0;
        description = "";
        itemName = "";
    }

    public void SetItem(GameObject item) {
        this.item = item.GetComponent<Item>();
        this.sprite = item.GetComponent<SpriteRenderer>().sprite;
        this.countItem += this.item.count;
        this.idItem = this.item.id;
        this.description = this.item.description;
        this.itemName = this.item.itemName;
    }

    public int DecreaseItemNumber() {
        this.countItem--;

        if (this.countItem == 0) {
            this.idItem = 0;
            this.item = null;
        }

        return this.countItem;
    }
}
