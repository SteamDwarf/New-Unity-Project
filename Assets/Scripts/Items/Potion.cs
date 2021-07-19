using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : Item
{
    [SerializeField] protected float increase;
    [SerializeField] protected float timeEffect;
    [SerializeField] protected AttributeType type;
    [SerializeField] protected EffectClass effectClass;
    [SerializeField] protected EffectType effectType;
    Player player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    public override void UseItem()
    {
        if(effectClass == EffectClass.none) {
            //player.UpdateHealth(increase);
            player.GetIncrease(type, increase);
            return;
        }

        if(effectClass != EffectClass.none) {
            player.GetEffect(effectClass,effectType, increase, timeEffect);
        }

        /* switch (type)
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
        } */

    }
}    
