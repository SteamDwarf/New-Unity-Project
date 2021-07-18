using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleverGhost : Enemy
{
    [SerializeField] private GameObject projectile;
    [SerializeField] List<float> attackDelays;
    [SerializeField] protected float maxMana;
    [SerializeField] protected float meleeAttackRadius;
    [SerializeField] protected float rangeAttackRadius;
    [SerializeField] protected float manaPerAttack;
    [SerializeField] protected float staminaPerAttack;
    [SerializeField] protected float findAliesCooldown;

    protected GameObject choosedAlies;
    protected EnemyBehaviourState curState;
    protected float curMana;
    protected bool findRecharging;
    protected bool isRecharging;

    

    protected override void Start() {
        base.Start();

        curState = EnemyBehaviourState.fullResources;
        curMana = maxMana;
    }
    protected override void Update() {
        base.Update();

        //CheckDistance();
    }

    protected void CheckDistance() {
        if(curState == EnemyBehaviourState.recharging) {
            return;
        }

        if(Vector2.Distance(rB.position, currentPlayerPosition) <= meleeAttackRadius) {
            curState = EnemyBehaviourState.playerClose;
            return;
        } else if (Vector2.Distance(rB.position, currentPlayerPosition) > meleeAttackRadius && curMana >= manaPerAttack && curState != EnemyBehaviourState.attacking) {
            curState = EnemyBehaviourState.canAttack;
            return;
        }

        if(Vector2.Distance(rB.position, currentPlayerPosition) > agroRange && curState == EnemyBehaviourState.lowResources) {
            curState = EnemyBehaviourState.recharging;
            return;
        }
    }
    protected override void DefaultBehavior() {
        /* if(curState == EnemyBehaviourState.recharging) {
            target = rB.position;
            RechargeMana();
            return;
        }
        if(curState == EnemyBehaviourState.playerClose || curState == EnemyBehaviourState.lowResources) {
            FindAlies();
                
            if(choosedAlies != null) {
                target = choosedAlies.GetComponent<Rigidbody2D>().position;
                Debug.Log(choosedAlies);
            } else if(stamina <  staminaPerAttack) {
                MeleeAttack();
            }

            return;   
        }

        if(sawPlayer) {
            Vector2 vectorNorm = (playerRB.position - rB.position).normalized;
            Flip(vectorNorm);

            if(curMana >= manaPerAttack && (curState == EnemyBehaviourState.canAttack || curState == EnemyBehaviourState.fullResources)) {
                RangeAttack(vectorNorm);
            }

            return;
        } else {
            if(curState == EnemyBehaviourState.canAttack) {
                curState = EnemyBehaviourState.lowResources;
            }
            return;
        } */

        if(isRecharging) {
            return;
        }

        if (currentAgroTime <= 0) {
            sawPlayer = false;
        }

        if (sawPlayer) {
            if(Vector2.Distance(rB.position, currentPlayerPosition) <= meleeAttackRadius && stamina >= staminaPerAttack) {
                if(choosedAlies == null || choosedAlies.GetComponent<Enemy>().isDied) {
                    FindAlies();
                }
                
                if(choosedAlies != null) {
                    target = choosedAlies.transform.position;
                    Debug.Log(choosedAlies);
                } else {
                    MeleeAttack();
                }

                return;
            }

            if(curMana >= manaPerAttack) {
                Vector2 vectorNorm = (playerRB.position - rB.position).normalized;
                Flip(vectorNorm);
                
                if(!isAttacking) {
                    RangeAttack(vectorNorm);
                }
                return;
            }

            if(curMana < manaPerAttack ) {

                if(findRecharging) {
                    if(Vector2.Distance(rB.position, target) < meleeAttackRadius) {
                        findRecharging = false;
                        choosedAlies = null;
                    }
                    return;
                }

                FindAlies();

                if(choosedAlies != null) {
                    target = choosedAlies.transform.position;
                    Debug.Log(choosedAlies);
                } else {
                    target = startPosition;
                }

                return;  
            }

        } else {
            if(curMana < maxMana && choosedAlies != null) {
                target = choosedAlies.transform.position;
            } else if(curMana < maxMana && choosedAlies == null){
                findRecharging = false;
                FindAlies();

                if(choosedAlies != null) {
                    target = choosedAlies.transform.position;
                } else {
                    target = startPosition;
                }
            }

            anim.SetState(true, "Walk");
            speed = defaultSpeed;

            if (Vector2.Distance(transform.position, target) < 1) {
                anim.SetState(false);

                if(curMana < maxMana) {
                    RechargeMana();
                }
            } 
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
        //Vector2 norm = (target - rB.position).normalized;
        instProjectile.GetComponent<ProjectileObject>().Launch(norm);
        curMana -= manaPerAttack;

        currentAgroTime = startAgroTime;
        anim.SetActing(curAttack);
        StartCoroutine(AttackCoroutine());
    }

    protected void FindAlies() {
        if(choosedAlies != null) {
            Enemy ally = choosedAlies.GetComponent<Enemy>();
            Rigidbody2D allyRB = choosedAlies.GetComponent<Rigidbody2D>(); 
            float safeDistance = Vector2.Distance(allyRB.position, currentPlayerPosition);

            if(safeDistance > agroRange) {
                return;
            }
        }

        Collider2D[] targets = Physics2D.OverlapCircleAll(rB.position, 100);

        foreach (Collider2D target in targets){
            if(target.GetComponent<Enemy>() != null && target.gameObject != this.gameObject && !target.GetComponent<Enemy>().isDied) {
                Rigidbody2D allyRB = target.GetComponent<Rigidbody2D>(); 
                float distance = Vector2.Distance(allyRB.position, playerRB.position);
                if(distance > agroRange) {
                    choosedAlies = target.gameObject;
                    findRecharging = true;
                }
            }
        }
    }

    protected void RechargeMana() {
        if (curMana < maxMana){
            //curState = EnemyBehaviourState.recharging;
            curMana += Time.deltaTime * 10;
            Debug.Log("Recharging");
        } else {
            //curState = EnemyBehaviourState.fullResources;
            Debug.Log("End Recharge");
        }
    }

    protected IEnumerator AttackCoroutine() {
        int delayIndex = Random.Range(0, attackDelays.Count);
        float delay = attackDelays[delayIndex];
        curState = EnemyBehaviourState.attacking;
        isAttacking = true;
        yield return new WaitForSeconds(delay);
/*         if(curMana >= manaPerAttack) {
            curState = EnemyBehaviourState.canAttack;
        } else {
            curState = EnemyBehaviourState.lowResources;
        } */
        
        isAttacking = false;
    }
}
