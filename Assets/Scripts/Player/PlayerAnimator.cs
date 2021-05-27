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
    public string faceTo;
    public bool isActing;
    public bool isMoving;
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
        if (!isActing && !isMoving)
            curAnimator.Play("player" + "Idle" + faceTo);
        else if (act == "Hurt")
            curAnimator.Play("player" + "Hurt" + faceTo);
        else if (isActing && isMoving)
            curAnimator.Play("player" + act + curState + faceTo);
        else if (isActing)
            curAnimator.Play("player" + act + faceTo);
        else if (isMoving)
            curAnimator.Play("player" + curState + faceTo);
        /*if (isAttacking)
            curAnimator.Play("player" + curAttack + faceTo);
        else if (isHurting)
            curAnimator.Play("player" + "Hurt" + faceTo);
        else if (getBlocking)
        {
            animIsBlocked = true;
            curAnimator.Play("player" + "Block" + faceTo);
            getBlocking = false;
            isBlocking = true;
            AnimationPlay();
        }
        else if (isBlocking)
        {
            curAnimator.Play("player" + "Blocking" + faceTo);
            animIsBlocked = false;
        }
        else
            curAnimator.Play("player" + curState + faceTo);*/
    }

}
