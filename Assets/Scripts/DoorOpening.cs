using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpening : MonoBehaviour
{
    [SerializeField] private AudioClip openCloseAudio;
    //private AudioSource audioSource;
    private Animator anim;
    //private LayerMask layerM;
    private SpriteRenderer spriteRend;
    private BoxCollider2D colliderDoor;
    private Player player;
    private AudioPlayer audioPlayer;
    private string typeDoor;
    private string doorState;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        //audioSource = GetComponent<AudioSource>();
        colliderDoor = GetComponent<BoxCollider2D>();
        spriteRend = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        audioPlayer = GetComponent<AudioPlayer>();
        //layerM = GameObject.GetComponent<LayerMask>();
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
            LayerMask.LayerToName(0);
            //layerM.value = 0;

        }
        else if(doorState == "Opened")
        {
            colliderDoor.isTrigger = false;
            spriteRend.sortingLayerName = "DoorFront";
            StartCoroutine(closeDoor());
            doorState = "Closed";
            LayerMask.LayerToName(3);
            //layerM.value = 0;
        }

        audioPlayer.PlayOneShot(openCloseAudio);
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

    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            player.TriggerDoor(this.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            player.TriggerDoor(this.gameObject);
        }
    }*/

}
