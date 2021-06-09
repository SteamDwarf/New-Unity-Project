using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
    public Vector2 target;
    public float speed = 200f;
    public float nextWaypiontDistance = 3f;
    public float agroRange;
    public float startAgroTime;

    private Path path;
    private int currentWaypoint = 0;
    private bool reachedEndOfPath = false;
    private bool sawPlayer = false;
    private float currentAgroTime;
    
    private Seeker seeker;
    private Rigidbody2D RB;
    private GameObject player;
    private Vector2 currentPlayerPosition;
    private Vector2 startPosition;

    void Start()
    {
        seeker = GetComponent<Seeker>();
        RB = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");

        agroRange = 4 * 10;
        startPosition = transform.position;
        InvokeRepeating("UpdatePath", 0f, 0.5f);
       
    }

    void FixedUpdate()
    {
        PlayerFind();
        if (path == null)
            return;

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - RB.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        RB.AddForce(force);

        float distance = Vector2.Distance(RB.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypiontDistance)
        {
            currentWaypoint++;
        }
    }

    private void UpdatePath()
    {
        if(seeker.IsDone())
            seeker.StartPath(RB.position, target, OnPathComplete);
    }

    private void OnPathComplete(Path p)
    {
        if(!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    private void PlayerFind()
    {
        if (Vector3.Distance(player.transform.position, this.gameObject.transform.position) < agroRange)
        {
            currentPlayerPosition = player.transform.position;
            currentAgroTime = startAgroTime;
            sawPlayer = true;
        }

        if(sawPlayer)
        {
            target = currentPlayerPosition;
            currentAgroTime -= Time.deltaTime;
        }else
        {
            target = startPosition;
        }
    }

}
