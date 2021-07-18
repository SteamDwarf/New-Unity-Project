using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ThrowingItem : Item
{
    [SerializeField] protected float damage;
    [SerializeField] protected float power;
    [SerializeField] protected ItemDamageType type;
    [SerializeField] protected bool afterLaunchDestroyable;
    [SerializeField] protected bool afterActionDistroyable;
    [SerializeField] protected bool hasActionAfterTime;
    [SerializeField] protected float time;
    [SerializeField] protected bool actionAfterConnect;
    protected Rigidbody2D rb;
    protected Player player;
    protected Animator anim;
    protected bool isLaunched;
    protected float currentPower;

    protected delegate void MakeAction();
    protected MakeAction actionDel;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }


    public override void UseItem() {}

    public virtual void Launch(Vector2 target) {
        actionDel = Action;

        currentPower = power;

        if(afterLaunchDestroyable) {
            isPicked = true;
        } else {
            DropItem();
        }

        if(rb == null) {
            rb = GetComponent<Rigidbody2D>();
        }
        if(anim == null) {
            anim = GetComponent<Animator>();
        }

        rb.AddForce(target * currentPower);
        anim.Play("Launched");

        StartCoroutine(LaunchCorutine());

        if(hasActionAfterTime) {
            StartCoroutine(ActionCaroutine());
        }
    }

    protected void Ricochet(Collider2D other) {
        if(other == null) {
            return;
        }

        Vector3 blockPos = other.GetComponent<Transform>().position;
        Vector2 distanceNorm = (rb.position - new Vector2(blockPos.x, blockPos.y)).normalized;

        rb.velocity = new Vector2(0, 0);
        currentPower = currentPower / 2;
        rb.AddForce(distanceNorm * currentPower);

        StopCoroutine(LaunchCorutine());
        StopCoroutine(RicochetCorutine());
        StartCoroutine(RicochetCorutine());
    }

    protected virtual void Action(){}

    protected IEnumerator LaunchCorutine() {
        isLaunched = true;
        yield return new WaitForSeconds(2f);
        rb.velocity = new Vector2(0, 0);
        rb.angularVelocity = 0;
        anim.Play("Droped");
        isLaunched = false;
    }

    protected IEnumerator RicochetCorutine() {
        isLaunched = true;
        yield return new WaitForSeconds(0.5f);
        rb.velocity = new Vector2(0, 0);
        rb.angularVelocity = 0;
        anim.Play("Droped");
        isLaunched = false;
    }

    protected IEnumerator ActionCaroutine() {
        yield return new WaitForSeconds(time);

        if(actionDel != null) {
            actionDel();
        }
    }

}
