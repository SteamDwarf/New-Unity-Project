using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : Enemy
{

    protected override void MakeAttack() {
        int attackInd = Random.Range(0, enemyAttacks.Count);
        Attack attack = enemyAttacks[attackInd];
        //attackTime = 1f;
        stamina -= 30;
        curAttack = attack.name;
        currentAgroTime = startAgroTime;
        anim.SetActing(curAttack);
        //StartCoroutine(AttackCoroutine());
        //attackPoses[attackInd].GetComponent<HitBox>().damage = attack.damage;
    }


    protected override void DefaultBehavior() {
        if (sawPlayer) {
            target = currentPlayerPosition;
            anim.SetState(true, "Run");
            speed = defaultSpeed * 2;
            currentAgroTime -= Time.deltaTime;

            if (stamina >= 30f && !anim.isActing && Vector2.Distance(transform.position, target) <= attackRadius && !isAttacking) {
                MakeAttack();
            }
        }else {
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

/*     protected IEnumerator AttackCoroutine() {
        isAttacking = true;
        yield return new WaitForSeconds(2f);
        isAttacking = false;
    } */
}
