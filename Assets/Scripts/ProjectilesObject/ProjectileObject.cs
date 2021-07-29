using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ProjectileObject : MonoBehaviour
{
    [SerializeField] protected float damage;
    [SerializeField] protected float power;
    [SerializeField] protected bool hasActionAfterTime;
    [SerializeField] protected float time;
    [SerializeField] protected bool actionAfterConnect;
    [SerializeField] protected EffectClass effectClass;
    [SerializeField] protected EffectType effectType;
    [SerializeField] float increaseEffect;
    [SerializeField] float timeEffect; 
    protected Rigidbody2D rb;
    private bool isParried;
    public UnityEvent<Collider2D, float> ActionDelegate;


    public void Launch(Vector2 target) {
        if(rb == null) {
            rb = GetComponent<Rigidbody2D>();
        }

        rb.AddForce(target * power);
        //anim.Play("Launched");
    }

    protected void OnTriggerEnter2D(Collider2D other) {
        if(other.GetComponent<HitBox>() != null) {
            Vector2 hitboxPosition = other.gameObject.transform.position;
            Vector2 vectorNorm = (rb.position - hitboxPosition).normalized;

            rb.velocity = Vector2.zero;
            rb.AddForce(vectorNorm * power);
            isParried = true;
            return;
        }


        if(actionAfterConnect) {
            if(other.GetComponent<Player>() != null && !other.GetComponent<Player>().isDied) {
                ActionDelegate.Invoke(other, damage);
                other.GetComponent<Player>().GetEffect(effectClass, effectType, increaseEffect, timeEffect);
                Destroy(this.gameObject);
                return;
            }
            if(other.GetComponent<Ghost>() != null && !other.GetComponent<Ghost>().isDied && isParried) {
                ActionDelegate.Invoke(other, damage);
                Destroy(this.gameObject);
                return;
            }
            
        }

        if(other.CompareTag("Wall")) {
            Destroy(this.gameObject);
        }    
    }
}
