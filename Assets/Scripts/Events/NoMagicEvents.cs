using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoMagicEvents : MonoBehaviour
{
    public void SetSimpleDamageTarget(Collider2D other, float damage) {
        other.GetComponent<IGetDamage>().GetDamage(damage);
    }
    public void SetKnockbackableDamageTarget(Vector2 position, Collider2D other, float damage, float knockPower) {
        other.GetComponent<IGetDamage>().GetDamage(damage);
        
        if(other.GetComponent<IKnockbackable>() != null) {
            Vector2 targetPos = other.GetComponent<Rigidbody2D>().position;
            Vector2 knockVector = (targetPos - position).normalized;
            other.GetComponent<IKnockbackable>().Knockback(knockVector, knockPower);
        }
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
    public void SetKnockbackableDamageArea(Vector2 position, float radius, float damage, float knockPower) {
        Collider2D[] targets = Physics2D.OverlapCircleAll(position, radius);

        foreach (Collider2D target in targets){
            if(target.GetComponent<IGetDamage>() != null) {
                target.GetComponent<IGetDamage>().GetDamage(damage);
            }
            if(target.GetComponent<IKnockbackable>() != null) {
                Vector2 targetPos = target.GetComponent<Rigidbody2D>().position;
                Vector2 knockVector = (targetPos - position).normalized;
                target.GetComponent<IKnockbackable>().Knockback(knockVector, knockPower);
            }
        }
    }
}
