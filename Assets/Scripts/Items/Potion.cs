using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : Item
{
    [SerializeField] protected float increase;
    [SerializeField] protected float timeEffect;
    [SerializeField] protected PotionType type;
    Player player;

    new void Start()
    {
        base.Start();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    public override void UseItem()
    {
        switch (type)
        {
            case PotionType.health:
                player.UpdateHealth(increase);
                break;
            case PotionType.stamina:
                player.GetContiniousEffect(increase, timeEffect, type);
                break;
            case PotionType.strength:
                player.GetContiniousEffect(increase, timeEffect, type);
                break;
            case PotionType.speed:
                player.GetContiniousEffect(increase, timeEffect, type);
                break;
        }

        //Destroy(this.gameObject);
    }

    public override void SetInformation(Dictionary<string, object> information){
        this.id = (int)information["id"];
        this.description = (string)information["description"];
        this.itemName = (string)information["itemName"];
        this.count = (int)information["count"];
        this.increase = (float)information["increase"];
        this.timeEffect = (float)information["timeEffect"];
        this.type = (PotionType)information["type"];
        this.useType = (ItemUseType)information["useType"];
    }

    public override Dictionary<string, object> GetItemInformation() {
        Dictionary<string, object> information = new Dictionary<string, object> {
            {"id", this.id},
            {"description", this.description},
            {"itemName", this.itemName},
            {"count", this.count},
            {"increase", this.increase},
            {"timeEffect", this.timeEffect},
            {"type", this.type},
            {"collider", GetComponent<CapsuleCollider2D>()},
            {"useType", this.useType}
        };

        return information;
    }

/*     public override Item Clone(){
        Potion clone = new Potion();

        clone.id = this.id;
        clone.description = this.description;
        clone.itemName = this.itemName;
        clone.count = this.count;
        clone.increase = this.increase;
        clone.timeEffect = this.timeEffect;
        clone.type = this.type;

        return clone;
    } */
}    
