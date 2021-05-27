using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    Animator curAnimator;
   
    public string curState;
    public bool isAttacking;
    public bool isHurting;
    public string curAttack;

    private string enemyName;

    void Start()
    {

        curAnimator = GetComponent<Animator>();
        curState = "Idle";
        enemyName = gameObject.name.Split('(')[0];
    }

    private void FixedUpdate()
    {
        AnimationPlay();
    }

    private void AnimationPlay()
    {
        if (curState == "Dying" && !isHurting)
        {
            curAnimator.Play("Die" + enemyName);
            curState = "Died";
        }
        else if (curState == "Died")
            curAnimator.Play("Died" + enemyName);
        else if (isAttacking)
        {
            curAnimator.Play(curAttack + enemyName);
            //Debug.Log(curAttack);
            //Debug.Log(enemyName);
        }
        else if (isHurting)
            curAnimator.Play("Hurt" + enemyName);
        else
            curAnimator.Play(curState + enemyName);

    }
}
