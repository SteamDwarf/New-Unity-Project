using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaStealer : Enemy
{
    [SerializeField] private GameObject projectile;
    [SerializeField] List<float> attackDelays;
    [SerializeField] protected float maxMana;
    [SerializeField] protected float meleeAttackRadius;
    [SerializeField] protected float manaPerAttack;
    [SerializeField] protected float staminaPerAttack;
    [SerializeField] protected float findAliesCooldown;

    protected List<GameObject> enemies;
    protected GameObject choosedAlies;
    protected float curMana;
    protected bool findRecharging;
    

    protected override void Start() {
        base.Start();

        curMana = maxMana;
    }
    protected override void Update() {
        base.Update();
        
        if(choosedAlies != null && Vector2.Distance(rB.position, choosedAlies.transform.position) < meleeAttackRadius) {
            GetManaFromAlies();
        }
    }

     protected override void MakeAttack()
    {

        if(Vector2.Distance(rB.position, target) <= meleeAttackRadius && stamina >= staminaPerAttack && !isAttacking ) {
            MeleeAttack();
            return;
        }

        if(Vector2.Distance(rB.position, target) > meleeAttackRadius && curMana >= manaPerAttack && !isAttacking) {
            RangeAttack();
            return;
        }
    }

    protected override void DefaultBehavior()
    {
        if (sawPlayer) {
            if(curMana >= manaPerAttack) {
                target = currentPlayerPosition;
                if(!isAttacking) {
                    RangeAttack();
                }
                return;
            }

            if(Vector2.Distance(rB.position, currentPlayerPosition) <= meleeAttackRadius && stamina >= staminaPerAttack && !isAttacking ) {
                MeleeAttack();
                return;
            }

            if(curMana < manaPerAttack /* && stamina >= staminaPerAttack */) {
                if(isAttacking || findRecharging) {
                    return;
                }

                int enemyInd;
                FindAlies();

                if(enemies.Count > 0) {
                    enemyInd = Random.Range(0, enemies.Count);
                    choosedAlies = enemies[enemyInd];
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
            } else {
                target = startPosition;
            }

            anim.SetState(true, "Walk");
            speed = defaultSpeed;

            if (Vector2.Distance(transform.position, target) < 1)
                anim.SetState(false);
        }

        if (currentAgroTime <= 0)
        {
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
    protected void RangeAttack() {
        GameObject instProjectile = Instantiate(projectile, rB.position, Quaternion.identity);
        Vector2 norm = (target - rB.position).normalized;
        instProjectile.GetComponent<ProjectileObject>().Launch(norm);
        curMana -= manaPerAttack;

        currentAgroTime = startAgroTime;
        anim.SetActing(curAttack);
        StartCoroutine(AttackCoroutine());

    }

    protected void FindAlies() {
        Collider2D[] targets = Physics2D.OverlapCircleAll(rB.position, 30);
        enemies = new List<GameObject>();

        foreach (Collider2D target in targets){
            if(target.GetComponent<Enemy>() != null && target.gameObject != this.gameObject && !target.GetComponent<Enemy>().isDied) {
                enemies.Add(target.gameObject);
            }
        }

        StartCoroutine(FindAliesCoroutine());
    }
    protected void GetManaFromAlies() {
        choosedAlies.GetComponent<Enemy>().GetDamage(1);
        curMana++;

        if(choosedAlies.GetComponent<Enemy>().isDied) {
            choosedAlies = null;
            findRecharging = false;
        }
    }

    protected IEnumerator AttackCoroutine() {
        int delayIndex = Random.Range(0, attackDelays.Count);
        float delay = attackDelays[delayIndex];
        isAttacking = true;
        yield return new WaitForSeconds(delay);
        isAttacking = false;
    }

    protected IEnumerator FindAliesCoroutine() {
        findRecharging = true;
        yield return new WaitForSeconds(findAliesCooldown);
        findRecharging = false;

    }
}
