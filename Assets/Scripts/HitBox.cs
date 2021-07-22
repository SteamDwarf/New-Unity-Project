using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    public float damage;
    public float thrust;
    public string owner;

    private PolygonCollider2D coll;

    private void Start() {
        coll = GetComponent<PolygonCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        MakeAttack(collision); 
    }


    private void MakeAttack(Collider2D collision) {
        GameObject hitTarget = collision.gameObject;

        if (hitTarget.GetComponent<Player>() != null && owner != "Player") {
            Player playerScript = hitTarget.GetComponent<Player>();

            if (playerScript.isDied)
                return;

            playerScript.GetDamage(damage);
        } else if (hitTarget.GetComponent<Enemy>() != null && owner != "Enemy") {
            Vector2 hitBoxPos = new Vector2(transform.position.x, transform.position.y);
            Vector2 enemyPos = hitTarget.GetComponent<Rigidbody2D>().position;
            Enemy enemyScript = hitTarget.GetComponent<Enemy>();
            Vector2 knockVector = (enemyPos - hitBoxPos).normalized;

            if (enemyScript.isDied)
                return;

            enemyScript.GetDamage(damage);
            enemyScript.Knockback(knockVector, 400);
        }
    }
}
