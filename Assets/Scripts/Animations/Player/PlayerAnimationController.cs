using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private GameObject playerFront;
    [SerializeField] private GameObject playerBack;
    [SerializeField] private GameObject playerLeft;
    [SerializeField] private GameObject playerRight;

    [SerializeField] AnimationUnit beginDieAnimation;
    [SerializeField] AnimationUnit dieAnimation;
    [SerializeField] AnimationUnit idleAnimation;

    private Animator frontAnimator;
    private Animator backAnimator;
    private Animator leftAnimator;
    private Animator rightAnimator;

    private Animator currentAnimator;
    public AnimationUnit movingState {get; private set;}
    public AnimationUnit actingState {get; private set;}
    public AnimationUnit currentActing {get; private set;}

    private bool isDied;
    private bool isBlocked;
    private PlayerFaceTo currentFaceTo;
    // Start is called before the first frame update
    public void Start(){
        frontAnimator = playerFront.GetComponent<Animator>();
        backAnimator = playerBack.GetComponent<Animator>();
        leftAnimator = playerLeft.GetComponent<Animator>();
        rightAnimator = playerRight.GetComponent<Animator>();

        currentFaceTo = PlayerFaceTo.front;
        currentAnimator = frontAnimator;

        ActivateCurrentFace(playerFront);

        isDied = false;
    }

    // Update is called once per frame
    void Update() {
        PlayAnimation();
    }

    public void ChangeFaceTo(Vector2 direction) {
        float num = Mathf.Abs(direction.x) > Mathf.Abs(direction.y) ? direction.x : direction.y; 
        string side = Mathf.Abs(direction.x) > Mathf.Abs(direction.y) ? "x" : "y";

        if(side == "x") {
            if(num > 0) {
                currentFaceTo = PlayerFaceTo.right;
                currentAnimator = rightAnimator;
                ActivateCurrentFace(playerRight);
            } else {
                currentFaceTo = PlayerFaceTo.left;
                currentAnimator = leftAnimator;
                ActivateCurrentFace(playerLeft);
            }
        } else {
            if(num > 0) {
                currentFaceTo = PlayerFaceTo.back;
                currentAnimator = backAnimator;
                ActivateCurrentFace(playerBack);
            } else {
                currentFaceTo = PlayerFaceTo.front;
                currentAnimator = frontAnimator;
                ActivateCurrentFace(playerFront);
            }
        }

/*         if (direction.y > 0) {
            currentFaceTo = PlayerFaceTo.back;
            currentAnimator = backAnimator;
            ActivateCurrentFace(playerBack);
        }else if (direction.y < 0) {
            currentFaceTo = PlayerFaceTo.front;
            currentAnimator = frontAnimator;
            ActivateCurrentFace(playerFront);
        }else if (direction.x > 0) {
            currentFaceTo = PlayerFaceTo.right;
            currentAnimator = rightAnimator;
            ActivateCurrentFace(playerRight);
        }else if (direction.x < 0) {
            currentFaceTo = PlayerFaceTo.left;
            currentAnimator = leftAnimator;
            ActivateCurrentFace(playerLeft);
        } */
    }

    public void ChangeAnimation(AnimationUnit anim) {
        if(currentActing != null) {
            return;
        }

        if(!anim.isState) {
            currentActing = anim;
            StartCoroutine(ActingCorutine(currentActing.time));

            if(!anim.isMixing || !movingState.isMixing || !actingState.isMixing) {
                movingState = null;
                actingState = null;
            }
        } else {
/*             if(currentActing != null && !currentActing.isMixing) {
                return;
            } */
            if(movingState != null && !movingState.isMixing) {
                return;
            }
            if(actingState != null && !actingState.isMixing) {
                return;
            }

            if(anim.isMovingState) {
                movingState = anim;
            } else {
                actingState = anim;
            }
        }
    }

    public void PlayAnimation() {
/*         if(isBlocked) {
            return;
        } */

        string currentAnimation = "";

        if(movingState != null) {
            currentAnimation += movingState.animationName;
        }
        if(actingState != null) {
            currentAnimation += actingState.animationName;
        }
        if(currentActing != null) {
            currentAnimation += currentActing.animationName;
        }

        currentAnimator.Play(currentAnimation);

    }

    public void ClearActingState() {
        actingState = null;
    }

    private IEnumerator ActingCorutine (float time) {
        //isBlocked = true;
        yield return new WaitForSeconds(time);
        currentActing = null;
        //isBlocked = false;
    } 
    private void ActivateCurrentFace(GameObject face) {
        playerFront.SetActive(false);
        playerBack.SetActive(false);
        playerLeft.SetActive(false);
        playerRight.SetActive(false);

        face.SetActive(true);
    }
}
