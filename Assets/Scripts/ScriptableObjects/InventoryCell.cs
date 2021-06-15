using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class InventoryCell : ScriptableObject
{
    public GameObject item;
    public Sprite sprite;
    public int countItem;
    public int idItem;
    public string description;
    public string itemName;

    public InventoryCell Clone()
    {
        InventoryCell copyCell = ScriptableObject.CreateInstance<InventoryCell>();

        copyCell.item = this.item;
        copyCell.sprite = this.sprite;
        copyCell.countItem = this.countItem;
        copyCell.idItem = this.idItem;
        copyCell.description = this.description;
        copyCell.itemName = this.itemName;

        return copyCell;
    }

    public void Clear()
    {
        item = null;
        sprite = null;
        countItem = 0;
        idItem = 0;
        description = "";
        itemName = "";
    }
}
