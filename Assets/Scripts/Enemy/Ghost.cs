using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : Enemy
{
    [SerializeField] private GameObject projectile;
    [SerializeField] List<float> attackDelays;
    [SerializeField] protected float maxMana;
    [SerializeField] protected float meleeAttackRadius;
    [SerializeField] protected float rangeAttackRadius;
    [SerializeField] protected float manaPerAttack;
    [SerializeField] protected float staminaPerAttack;

    protected float curMana;

    

    protected override void Start() {
        base.Start();

        curMana = maxMana;
    }
    protected override void Update() {
        base.Update();

        RechargeMana();
    }

    protected override void DefaultBehavior() {
        if(sawPlayer) {
            Vector2 vectorNorm = (playerRB.position - rB.position).normalized;

            if(Vector2.Distance(playerRB.position, rB.position) > rangeAttackRadius && !isAttacking) {
                target = currentPlayerPosition;
            } else {
                target = rB.position;
                Flip(vectorNorm);
            }

            if(Vector2.Distance(playerRB.position, rB.position) > meleeAttackRadius && !isAttacking) {
                RangeAttack(vectorNorm);
            } else if(Vector2.Distance(playerRB.position, rB.position) < meleeAttackRadius && !isAttacking){
                MeleeAttack();
            }
        } else {
            target = startPosition;
        }


        if (currentAgroTime <= 0) {
            sawPlayer = false;
        }
    }

    protected void MeleeAttack() {
        int attackInd = Random.Range(0, enemyAttacks.Count);
        Attack attack = enemyAttacks[attackInd];

        stamina -= staminaPerAttack;
        curAttack = attack.name;
        currentAgroTime = startAgroTime;
        anim.SetActing(curAttack);

        StartCoroutine(AttackCoroutine());
        //attackPoses[attackInd].GetComponent<HitBox>().damage = attack.damage;
    }
    protected void RangeAttack(Vector2 norm) {
        RaycastHit2D raycast = Physics2D.Raycast(rB.position, norm, rangeAttackRadius, LayerMask.GetMask("BlockingLayer"));
        if(raycast.collider != null) {
            if(raycast.collider.gameObject.GetComponent<Player>() == null) {
                return;
            }
        } else {
            return;
        }

        GameObject instProjectile = Instantiate(projectile, rB.position, Quaternion.identity);
        instProjectile.GetComponent<ProjectileObject>().Launch(norm);
        curMana -= manaPerAttack;

        currentAgroTime = startAgroTime;
        anim.SetActing("Cast");
        StartCoroutine(AttackCoroutine());
    }

    protected void RechargeMana() {
        if (curMana < maxMana)
        {
            if (anim.curState == "Idle")
                curMana += Time.deltaTime * 20;
            else if (anim.curState == "Run")
                curMana += Time.deltaTime * 10;
        }
    }

    protected IEnumerator AttackCoroutine() {
        int delayIndex = Random.Range(0, attackDelays.Count);
        float delay = attackDelays[delayIndex];
        isAttacking = true;
        yield return new WaitForSeconds(delay);
        isAttacking = false;
    }
}
