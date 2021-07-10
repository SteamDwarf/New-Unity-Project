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
    public ItemUseType useType;

    public InventoryCell Clone() {
        InventoryCell copyCell = ScriptableObject.CreateInstance<InventoryCell>();

        copyCell.item = this.item;
        copyCell.sprite = this.sprite;
        copyCell.countItem = this.countItem;
        copyCell.idItem = this.idItem;
        copyCell.description = this.description;
        copyCell.itemName = this.itemName;
        copyCell.useType = this.useType;

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
        Item newItem = item.GetComponent<Item>();
        int itemId = newItem.GetId();
        Debug.Log(itemId);
        Dictionary<string, object> itemInformation = ItemDataBase.GetItemInformation(itemId);

        this.item = newItem;
        this.sprite = Resources.Load<Sprite>((string)itemInformation["sprite"]);
        this.countItem += newItem.GetCount();
        this.idItem = itemId;
        this.description = (string)itemInformation["description"];
        this.itemName = (string)itemInformation["itemName"];
        this.useType = (ItemUseType)itemInformation["useType"];
    }

    public int DecreaseItemNumber() {
        this.countItem--;

        if (this.countItem == 0) {
            this.idItem = 0;
            this.item = null;
        }

        return this.countItem;
    }

    public int DecreaseItemNumber(int count) {
        this.countItem -= count;

        if (this.countItem == 0) {
            this.idItem = 0;
            this.item = null;
        }

        return this.countItem;
    }
}
