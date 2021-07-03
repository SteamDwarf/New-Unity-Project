using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    public float damage;
    public float thrust;
    public string owner;

    private PolygonCollider2D coll;

    private void Start()
    {
        coll = GetComponent<PolygonCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        MakeAttack(collision); 
    }

    /*private void OnTriggerStay2D(Collider2D collision)
    {
        GameObject hitTarget = collision.gameObject;

        if (hitTarget.CompareTag("Enemy") && owner != "Enemy")
        {
            Enemy enemyScript = hitTarget.GetComponent<Enemy>();
            //Rigidbody2D enemyRB = hitTarget.GetComponent<Rigidbody2D>();

            if (enemyScript.isDied)
                return;

            enemyScript.GetDamage(damage);
        }
    }*/

    private void MakeAttack(Collider2D collision)
    {
        GameObject hitTarget = collision.gameObject;

        if (hitTarget.CompareTag("Player") && owner != "Player")
        {
            Player playerScript = hitTarget.GetComponent<Player>();

            if (playerScript.isDied)
                return;

            playerScript.GetDamage(damage);
        }
        else if (hitTarget.CompareTag("Enemy") && owner != "Enemy")
        {
            Enemy enemyScript = hitTarget.GetComponent<Enemy>();
            //Rigidbody2D enemyRB = hitTarget.GetComponent<Rigidbody2D>();

            if (enemyScript.isDied)
                return;

            enemyScript.GetDamage(damage);
            /*Vector2 diff = enemyRB.transform.position - transform.position;
            diff = diff.normalized * thrust;
            Debug.Log(diff);
            enemyRB.AddForce(diff, ForceMode2D.Impulse);
            StartCoroutine(KnockBack(enemyRB));*/
        }
    }


    /*IEnumerator KnockBack(Rigidbody2D enemyRB)
    {
        yield return new WaitForSeconds(1f);
        enemyRB.velocity = Vector2.zero;
    }*/
}
