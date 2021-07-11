using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    Animator curAnimator;
    Player player;
    public GameObject playerFront;
    public GameObject playerBack;
    public GameObject playerLeft;
    public GameObject playerRight;

    Animator playerFrontAnim;
    Animator playerBackAnim;
    Animator playerLeftAnim;
    Animator playerRightAnim;

    public string curState;
    public string additionalState;
    public string faceTo;
    public bool isActing;
    public bool isMoving;
    private bool isAdditionalMoving;
    public bool isDied;
    public string act;
    /*public bool isAttacking;
    public bool isBlocking;
    public bool getBlocking;
    public bool isHurting;
    public string curAttack;*/

    private string prevFaceTo;
    public bool animIsBlocked;

    void Start()
    {
        player = GetComponent<Player>();
        /*playerFront = GameObject.Find("Player_Front");
        playerBack = GameObject.Find("Player_Back");
        playerLeft = GameObject.Find("Player_Left");
        playerRight = GameObject.Find("Player_Right");*/

        playerFrontAnim = playerFront.GetComponent<Animator>();
        playerBackAnim = playerBack.GetComponent<Animator>();
        playerLeftAnim = playerLeft.GetComponent<Animator>();
        playerRightAnim = playerRight.GetComponent<Animator>();

        curState = "Idle";
        faceTo = "Front";
        isActing = false;
        isMoving = false;
        animIsBlocked = false;
        isDied = false;
        prevFaceTo = faceTo;
        curAnimator = playerFrontAnim;

    }

    private void FixedUpdate()
    {
        //CheckAnimBlocked();
        ChangeFaceTo();
    }

    /*private void CheckAnimBlocked()
    {
        if (isAttacking && curAnimator.GetCurrentAnimatorStateInfo(0).IsName("player" + curAttack + prevFaceTo))
            animIsBlocked = true;
        else
            animIsBlocked = false;
    }*/

    private void ChangeFaceTo()
    {
        if (animIsBlocked)
            return;

        prevFaceTo = faceTo;
        switch (faceTo)
        {
            case "Front":
                playerBack.SetActive(false);
                playerLeft.SetActive(false);
                playerRight.SetActive(false);
                playerFront.SetActive(true);
                curAnimator = playerFrontAnim;
                break;
            case "Back":
                playerBack.SetActive(true);
                playerLeft.SetActive(false);
                playerRight.SetActive(false);
                playerFront.SetActive(false);
                curAnimator = playerBackAnim;
                break;
            case "Left":
                playerBack.SetActive(false);
                playerLeft.SetActive(true);
                playerRight.SetActive(false);
                playerFront.SetActive(false);
                curAnimator = playerLeftAnim;
                break;
            case "Right":
                playerBack.SetActive(false);
                playerLeft.SetActive(false);
                playerRight.SetActive(true);
                playerFront.SetActive(false);
                curAnimator = playerRightAnim;
                break;
        }

        AnimationPlay();
        //Debug.Log("player" + curState + faceTo);
    }

    private void AnimationPlay()
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
            else if(isActing && isMoving && isAdditionalMoving)
                curAnimator.Play(curState + additionalState + act);
            else if(isActing && isAdditionalMoving)
                curAnimator.Play(additionalState + act);
            else if(isMoving && isAdditionalMoving)
                curAnimator.Play(curState + additionalState);
            else if (isActing && isMoving)
                curAnimator.Play(curState + act);
            else if (isActing)
                curAnimator.Play(act);
            else if (isMoving)
                curAnimator.Play(curState);
            else if(isAdditionalMoving)
                curAnimator.Play(additionalState);
        }

        /* if(isDied)
        {
            if(isActing && (act == "Die" || act == "Died"))
                curAnimator.Play(act);    
            
        } */
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

    public void PlayerDie() {
        StartCoroutine(DieCorutine());
    }

    private IEnumerator ActingCorutine() {
        isActing = true;
        yield return new WaitForSeconds(0.4f);
        isActing = false;
        act = "";
    }

    private IEnumerator DieCorutine() {
        isDied = true;
        curAnimator.Play("Died");
        yield return new WaitForSeconds(0.3f);
        curAnimator.Play("Die");
    }
}
