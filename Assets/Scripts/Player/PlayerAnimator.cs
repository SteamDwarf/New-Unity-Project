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

    public string curState {get; private set;}
    public string additionalState {get; private set;}
    public PlayerFaceTo faceTo;
    public bool isActing {get; private set;}
    public bool isMoving {get; private set;}
    public bool isAdditionalMoving {get; private set;}
    public bool isDied {get; private set;}
    public string act {get; private set;}

    void Start()
    {
        player = GetComponent<Player>();

        playerFrontAnim = playerFront.GetComponent<Animator>();
        playerBackAnim = playerBack.GetComponent<Animator>();
        playerLeftAnim = playerLeft.GetComponent<Animator>();
        playerRightAnim = playerRight.GetComponent<Animator>();

        curState = "Idle";
        faceTo = PlayerFaceTo.front;
        isActing = false;
        isMoving = false;
        isDied = false;
        //prevFaceTo = faceTo;
        curAnimator = playerFrontAnim;

        ChangeFaceTo(faceTo);
    }

    private void FixedUpdate() {
        AnimationPlay();
    }

    public void ChangeFaceTo(PlayerFaceTo face) {
        faceTo = face;

        switch (faceTo)
        {
            case PlayerFaceTo.front:
                playerBack.SetActive(false);
                playerLeft.SetActive(false);
                playerRight.SetActive(false);
                playerFront.SetActive(true);
                curAnimator = playerFrontAnim;
                break;
            case PlayerFaceTo.back:
                playerBack.SetActive(true);
                playerLeft.SetActive(false);
                playerRight.SetActive(false);
                playerFront.SetActive(false);
                curAnimator = playerBackAnim;
                break;
            case PlayerFaceTo.left:
                playerBack.SetActive(false);
                playerLeft.SetActive(true);
                playerRight.SetActive(false);
                playerFront.SetActive(false);
                curAnimator = playerLeftAnim;
                break;
            case PlayerFaceTo.right:
                playerBack.SetActive(false);
                playerLeft.SetActive(false);
                playerRight.SetActive(true);
                playerFront.SetActive(false);
                curAnimator = playerRightAnim;
                break;
        }
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
