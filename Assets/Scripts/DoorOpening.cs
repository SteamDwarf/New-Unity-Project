using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpening : MonoBehaviour
{
    private Animator anim;
    private SpriteRenderer spriteRend;
    private BoxCollider2D colliderDoor;
    private string typeDoor;
    private string doorState;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        colliderDoor = GetComponent<BoxCollider2D>();
        spriteRend = GetComponent<SpriteRenderer>();
        typeDoor = gameObject.name.Split('(')[0];
        doorState = "Closed";
    }

    public void ChangeDoorState()
    {
        if(doorState == "Closed")
        {
            colliderDoor.isTrigger = true;
            spriteRend.sortingLayerName = "DoorTop";
            StartCoroutine(openDoor());
            doorState = "Opened";

        }
        else if(doorState == "Opened")
        {
            colliderDoor.isTrigger = false;
            spriteRend.sortingLayerName = "DoorFront";
            StartCoroutine(closeDoor());
            doorState = "Closed";
        }
    }

    private IEnumerator openDoor()
    {
        anim.Play(typeDoor + "_open");
        yield return new WaitForSeconds(0.15f);
        anim.Play(typeDoor + "_opened");
    }

    private IEnumerator closeDoor()
    {
        anim.Play(typeDoor + "_close");
        yield return new WaitForSeconds(0.15f);
        anim.Play(typeDoor + "_closed");
    }

    // Update is called once per frame
}
