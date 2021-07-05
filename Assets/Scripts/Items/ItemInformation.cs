using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* public interface ItemInformation {

} */

public class ItemInformation{
    public int id {get; set;}
    public string description {get; set;}
    public string itemName {get; set;}
    public int count {get; set;}
    public float increase {get; set;}
    public float timeEffect {get; set;}
    public PotionType type {get; set;}
    public CapsuleCollider2D collider {get; set;}

    public ItemInformation(int id, string description, string itemName, int count, float increase, float timeEffect, PotionType type, CapsuleCollider2D collider) {
        this.id = id;
        this.description = description;
        this.itemName = itemName;
        this.count = count;
        this.increase = increase;
        this.timeEffect = timeEffect;
        this.type = type;
        this.collider = collider;
    }
}
