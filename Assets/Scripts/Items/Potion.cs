using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum typeEnum
{
    health, strength, stamina, speed
}
public class Potion : MonoBehaviour
{
    public float increase;
    public float timeEffect;
    public typeEnum type;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();

            switch(type)
            {
                case typeEnum.health:
                    player.UpdateHealth(increase);
                    break;
                case typeEnum.stamina:
                    player.GetContiniousEffect(increase, timeEffect, type);
                    break;
                case typeEnum.strength:
                    player.GetContiniousEffect(increase, timeEffect, type);
                    break;
                case typeEnum.speed:
                    player.GetContiniousEffect(increase, timeEffect, type);
                    break;
            }

            Destroy(this.gameObject);
            
        }
    }
}    
