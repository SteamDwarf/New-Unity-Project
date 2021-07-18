using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoMagicEvents : MonoBehaviour
{
    public void SetSimpleDamageTarget(Collider2D other, float damage) {
        other.GetComponent<IGetDamage>().GetDamage(damage);
    }

/*     public void SetSimpleDamageArea(Collider2D[] others, float damage) {
        foreach (Collider2D target in others){
            if(target.GetComponent<IGetDamage>() != null) {
                target.GetComponent<IGetDamage>().GetDamage(damage);
            }
        }
    } */

    public void SetSimpleDamageArea(Vector2 position, float radius, float damage) {
        Collider2D[] targets = Physics2D.OverlapCircleAll(position, radius);

        foreach (Collider2D target in targets){
            if(target.GetComponent<IGetDamage>() != null) {
                target.GetComponent<IGetDamage>().GetDamage(damage);
            }
        }
    }
}
