using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneTargetDamageItem : ThrowingItem
{
    protected override void OnTriggerEnter2D(Collider2D other) {
        base.OnTriggerEnter2D(other);

        if(other.GetComponent<Enemy>() != null) {
            other.GetComponent<Enemy>().GetDamage(damage);
            Destroy(this.gameObject);
        } else if(other.CompareTag("Wall")) {
            Destroy(this.gameObject);
        }
        
    }
   /*  public override void SetInformation(Dictionary<string, object> information){
        this.id = (int)information["id"];
        this.description = (string)information["description"];
        this.itemName = (string)information["itemName"];
        this.count = (int)information["count"];
        this.damage = (float)information["damage"];
        this.type = (ItemDamageType)information["type"];
        this.useType = (ItemUseType)information["useType"];
    }

    public override Dictionary<string, object> GetItemInformation() {
        Dictionary<string, object> information = new Dictionary<string, object> {
            {"id", this.id},
            {"description", this.description},
            {"itemName", this.itemName},
            {"count", this.count},
            {"damage", this.damage},
            {"type", this.type},
            {"collider", GetComponent<CapsuleCollider2D>()},
            {"useType", this.useType}
        };

        return information;
    } */
}
