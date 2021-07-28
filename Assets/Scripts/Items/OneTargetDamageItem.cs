using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OneTargetDamageItem : ThrowingItem
{
    [SerializeField] private float knockPower;
    protected Collider2D enemyColl;
    public UnityEvent<Collider2D, float> ActionDelegate;
    public UnityEvent<Vector2, Collider2D, float, float> ActionDelegateImproved;
    protected override void OnTriggerEnter2D(Collider2D other) {
        base.OnTriggerEnter2D(other);
        
        if(actionAfterConnect && (other.GetComponent<Enemy>() != null && !other.GetComponent<Enemy>().isDied)) {
            enemyColl = other;
            
            if(actionDel != null && isLaunched) {
                actionDel.Invoke();
            }
            return;
        }

        if(other.CompareTag("Wall") && isLaunched) {
            Ricochet(other);
            return;
        }

        if((other.GetComponent<Enemy>() != null && !other.GetComponent<Enemy>().isDied) && !actionAfterConnect) {
            Ricochet(other);
            return;
        }
        
    }

    protected override void Action() {
        if(knockPower == 0) {
            if(ActionDelegate != null) {
                ActionDelegate.Invoke(enemyColl, damage);
            }
        } else if(knockPower > 0) {
            if(ActionDelegateImproved != null) {
                ActionDelegateImproved.Invoke(rb.position, enemyColl, damage, knockPower);
            }
        }

        if(endActionEffect != null) {
            GameObject instEffect = Instantiate(endActionEffect, transform.position, Quaternion.identity);
        }

        if(afterLaunchDestroyable) {
            Destroy(this.gameObject);
            return;
        }

        if(afterActionDistroyable) {
            Destroy(this.gameObject);
            return;
        }

    }

}

