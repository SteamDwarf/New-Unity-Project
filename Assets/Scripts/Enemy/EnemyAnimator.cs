using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    private Animator curAnimator;

    public string curState {get; private set;}
    public string additionalState {get; private set;}
    public bool isActing {get; private set;}
    public bool isMoving {get; private set;}
    public bool isAdditionalMoving {get; private set;}
    public bool isDied {get; private set;}
    public string act {get; private set;}

    void Start()
    {
        curAnimator = GetComponent<Animator>();

        curState = "Idle";
        isActing = false;
        isMoving = false;
        isDied = false;
    }

    private void FixedUpdate() {
        AnimationPlay();
    }

    public void AnimationPlay()
    {
        if(isDied) {
            return;
        }

        if(!isDied)
        {  
            if (!isActing && !isMoving && !isAdditionalMoving)
                curAnimator.Play("Idle");
            else if (act == "Hurt")
                curAnimator.Play("Hurt");
           /*  else if(isActing && isMoving && isAdditionalMoving)
                curAnimator.Play(curState + additionalState + act);
            else if(isActing && isAdditionalMoving)
                curAnimator.Play(additionalState + act);
            else if(isMoving && isAdditionalMoving)
                curAnimator.Play(curState + additionalState);
            else if (isActing && isMoving)
                curAnimator.Play(curState + act); */
            else if (isActing)
                curAnimator.Play(act);
            else if (isMoving)
                curAnimator.Play(curState);
            else if(isAdditionalMoving)
                curAnimator.Play(additionalState);
        }
    }

    public void SetActing(string acting) {
        act = acting;
        StartCoroutine(ActingCorutine());
    }
    public void SetState(bool addState, string state = "") {
        if(addState) {
            isMoving = true;
            curState = state;
        } else {
            isMoving = false;
        }
    }
    public void SetAdditionalState(bool addState, string state = "") {
        if(addState) {
            isAdditionalMoving = true;
            additionalState = state;
        } else {
            isAdditionalMoving = false;
        }
    }

    public void CreatureDie() {
        StartCoroutine(DieCorutine());
    }

    private IEnumerator ActingCorutine() {
        isActing = true;
        yield return new WaitForSeconds(0.5f);
        isActing = false;
        act = "";
    }

    private IEnumerator DieCorutine() {
        isDied = true;
        curAnimator.Play("Die");
        yield return new WaitForSeconds(0.3f);
        curAnimator.Play("Died");
    }
}
