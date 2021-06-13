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
}
