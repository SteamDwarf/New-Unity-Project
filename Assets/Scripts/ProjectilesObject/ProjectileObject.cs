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
    [SerializeField] AttributeType effectType;
    [SerializeField] AttributeValueType effectValueType;
    [SerializeField] float increaseEffect;
    [SerializeField] float timeEffect; 
    protected Rigidbody2D rb;
    public UnityEvent<Collider2D, float> ActionDelegate;


    public void Launch(Vector2 target) {
        if(rb == null) {
            rb = GetComponent<Rigidbody2D>();
        }

        rb.AddForce(target * power);
        //anim.Play("Launched");
    }

    protected void OnTriggerEnter2D(Collider2D other) {
        
        if(actionAfterConnect && (other.GetComponent<Player>() != null && !other.GetComponent<Player>().isDied)) {
            ActionDelegate.Invoke(other, damage);
            other.GetComponent<Player>().GetEffect(effectType, effectValueType, increaseEffect, timeEffect);
            Destroy(this.gameObject);
            return;
        }

        if(other.CompareTag("Wall")) {
            Destroy(this.gameObject);
        }    
    }
}
