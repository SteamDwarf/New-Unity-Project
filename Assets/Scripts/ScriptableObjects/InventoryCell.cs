using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class InventoryCell : ScriptableObject
{
    public Item item;
    public bool isEmpty;
    public Sprite sprite;
}
