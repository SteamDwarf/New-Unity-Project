using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigAngryEnemy : Enemy
{
    [SerializeField] private float maxPower;
    [SerializeField] private float powerPerAction;
    private bool isJump;
    private float power;
    

    protected override void Start() {
        base.Start();

        power = maxPower;
    }
    protected override void Update()
    {
        base.Update();

        RefreshPower();
    }
    protected override void DefaultBehavior() {
        if(isJump) {
            return;
        }
        if(sawPlayer) {
            if(Vector2.Distance(transform.position, target) > attackRadius) {
                int actionLucky = Random.Range(0,5);
                Debug.Log(actionLucky);
                if(actionLucky < 3) {
                    /* Vector2 direction = (playerRB.position - rB.position).normalized;
                    speed = defaultSpeed * 2;
                    Vector2 force = direction * speed * Time.deltaTime;;
                    Move(force); */
                    target = currentPlayerPosition;
                    anim.SetState(true, "Run");
                    speed = defaultSpeed * 2;
                    currentAgroTime -= Time.deltaTime;
                }else if(actionLucky >= 3 && power >= powerPerAction) {
                    Jump();
                }

                if (stamina >= 30f && !anim.isActing && Vector2.Distance(transform.position, target) <= attackRadius && !isAttacking) {
                    MakeAttack();
                }
            } else {
                target = startPosition;
                anim.SetState(true, "Walk");
                speed = defaultSpeed;

                if (Vector2.Distance(transform.position, target) < 1) {
                    anim.SetState(false);
                }
            }

            if (currentAgroTime <= 0) {
                sawPlayer = false;
            }
        }
    }

    protected void Jump() {
        Vector2 direction = (playerRB.position - rB.position).normalized;
        Vector2 force;

        speed = defaultSpeed * 4;
        force = direction * speed * Time.deltaTime;

        Flip(direction);
        anim.SetTripleQueuedAction("JumpStart", "JumpLoop", "JumpEnd", 0.5f);
        rB.AddForce(force);

        currentAgroTime -= Time.deltaTime;
        power = 0;

        StartCoroutine(JumpCoroutine());
    }

    private IEnumerator JumpCoroutine() {
        isJump = true;
        yield return new WaitForSeconds(1.5f);
        isJump = false;
    }

    private void RefreshPower() {
        power += Time.deltaTime;
    }
}
