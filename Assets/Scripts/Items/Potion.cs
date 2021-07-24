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
    [SerializeField] private AudioClip useItemAudio;

    private GameObject playerGO;
    private Player player;

    void Start() {
        playerGO = GameObject.FindGameObjectWithTag("Player");
        player = playerGO.GetComponent<Player>();
    }

    public override void UseItem()
    {
        if(effectClass == EffectClass.none) {
            //player.UpdateHealth(increase);
            player.GetIncrease(type, increase);
        } else if(effectClass != EffectClass.none) {
            player.GetEffect(effectClass,effectType, increase, timeEffect);
        }

        player.GetComponent<AudioPlayer>().PlayOneShot(useItemAudio);
    }
}    
