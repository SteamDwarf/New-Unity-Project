using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ItemPrefabBuilder
{

    static GameObject CreateItemBase() {
        GameObject itemBase = new GameObject();
        itemBase.AddComponent<SpriteRenderer>();
        itemBase.GetComponent<SpriteRenderer>().sortingLayerName = "Items";
        itemBase.GetComponent<SpriteRenderer>().spriteSortPoint = SpriteSortPoint.Pivot;
        itemBase.GetComponent<Transform>().localScale = new Vector3(10, 10, 1);

        return itemBase; 
    }
    public static GameObject BuildPotionPrefab(Item itemScript, int count, Sprite sprite) {
        GameObject itemBase = CreateItemBase();
        //ItemInformation itemInformation = itemScript.GetItemInformation();
        Dictionary<string, object> itemInformation = itemScript.GetItemInformation();
        itemInformation["count"] = count;

        itemBase.AddComponent<Potion>();
        itemBase.GetComponent<Potion>().SetInformation(itemInformation);
        itemBase.AddComponent<CapsuleCollider2D>().isTrigger = true;

        CapsuleCollider2D collider = (CapsuleCollider2D)itemInformation["collider"];
        itemBase.GetComponent<CapsuleCollider2D>().size = collider.size;
        itemBase.GetComponent<CapsuleCollider2D>().offset = collider.offset;
        itemBase.GetComponent<SpriteRenderer>().sprite = sprite;

        return itemBase;
    }
}
