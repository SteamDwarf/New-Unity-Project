using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AreaDamageItem : ThrowingItem
{
    [SerializeField] protected ParticleSystem lifeTimeParticles;
    [SerializeField] protected float radius;
    [SerializeField] private float knockPower;

    public UnityEvent<Vector2, float, float> ActionDelegate;
    public UnityEvent<Vector2, float, float, float> ActionDelegateImproved;

    private void Start() {
        if(lifeTimeParticles != null && !isLaunched) {
            lifeTimeParticles.Stop();
        }
    }
    protected override void OnTriggerEnter2D(Collider2D other) {
        base.OnTriggerEnter2D(other);
        
        if(actionAfterConnect && (other.GetComponent<Enemy>() != null && !other.GetComponent<Enemy>().isDied)) {
            actionDel.Invoke();
            return;
        }

        if(other.CompareTag("Wall")) {
            Ricochet(other);
            return;
        }

        if((other.GetComponent<Enemy>() != null && !other.GetComponent<Enemy>().isDied) && !actionAfterConnect) {
            Ricochet(other);
            return;
        }
        
    }

    public override void Launch(Vector2 target)
    {
        base.Launch(target);
        //isPicked = true;

        if(lifeTimeParticles != null && isLaunched) {
            lifeTimeParticles.Play();
        }

        //StartCoroutine(BoomCaroutine());
        //MakeAction.AddListener(Action);
    }

    protected override void Action() {
        if(knockPower == 0) {
            if(ActionDelegate != null) {
                ActionDelegate.Invoke(rb.position, radius, damage);
            }
        } else if(knockPower > 0) {
            if(ActionDelegateImproved != null) {
                ActionDelegateImproved.Invoke(rb.position, radius, damage, knockPower);
            }
        }

        if(endActionEffect != null) {
            GameObject instEffect = Instantiate(endActionEffect, transform.position, Quaternion.identity);
        }

        if(afterLaunchDestroyable) {
            Destroy(this.gameObject);
        }

    }

/*     protected void  MakeBoom() {
        Collider2D[] targets = Physics2D.OverlapCircleAll(rb.position, 10);

        foreach (Collider2D target in targets){
            if(target.GetComponent<IGetDamage>() != null && MakeSimpleAction != null) {
                MakeSimpleAction.Invoke(target, damage);
            }
        }

        GameObject instEffect = Instantiate(endActionEffect, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    } */

    /* protected IEnumerator BoomCaroutine() {
        yield return new WaitForSeconds(3f);
        MakeBoom();
    } */
}
