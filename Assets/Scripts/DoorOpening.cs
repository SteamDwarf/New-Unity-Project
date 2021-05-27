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
            anim.Play(typeDoor + "_open");
            anim.Play(typeDoor + "_opened");
            doorState = "Opened";

        }
        else if(doorState == "Opened")
        {
            colliderDoor.isTrigger = false;
            spriteRend.sortingLayerName = "DoorFront";
            anim.Play(typeDoor + "_close");
            anim.Play(typeDoor + "_closed");
            doorState = "Closed";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
