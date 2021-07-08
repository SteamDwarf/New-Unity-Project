using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    private MapDrawer mapDrawer;
    private  GameObject gameManager;
    private Collider2D enemyCollider;
    protected EnemyAnimator anim;

    public GameObject healthBarCanvas;
    public Image healthBar;
    public Vector2 currentPlayerPosition;
    public List<GameObject> attackPoses;
    public List<float> attackRanges;
    public List<int> attackDamages;
    public float agroRange;
    //public EnemyState currentState;


    public float defaultSpeed;
    public float maxHealth;
    public bool sawPlayer;
    public float startAgroTime;
    public int maxStamina;
    public float attackRadius;
    public bool isDied;
    public bool isBig;
    public float nextWaypiontDistance;

    protected Rigidbody2D rB;
    //protected Animator anim;
    protected Vector2 moveVelocity;
    protected Vector2 startPosition;
    protected Vector2 target;
    protected List<Attack> enemyAttacks;
    protected GameManager GM;
    protected GameObject player;
    protected Path path;
    protected Seeker seeker;
    protected SortingGroup sortGr;


    protected int xCord;
    protected int yCord;
    protected string faceTo;
    protected float mapk;
    //protected string enemyName;
    protected float currentAgroTime;
    protected float speed;
    protected float health;
    protected float stamina;
    protected string currentAnimation;
    protected float attackTime;
    protected string curAttack;
    protected int currentWaypoint = 0;
    protected bool reachedEndOfPath = false;

    // TODO: При смерти врага HealthBar его должен пропадать и при повороте врага он не должен поворачиваться

    /////////Start, Update, FixedUpdate//////////////////////

    protected void Start()
    {
        gameManager = GameObject.Find("GameManager");
        player = GameObject.FindGameObjectWithTag("Player");
        rB = GetComponent<Rigidbody2D>();
        anim = GetComponent<EnemyAnimator>();
        seeker = GetComponent<Seeker>();
        GM = gameManager.GetComponent<GameManager>();
        mapDrawer = gameManager.GetComponent<MapDrawer>();
        enemyCollider = gameObject.GetComponent<Collider2D>();
        sortGr = GetComponent<SortingGroup>();
        mapk = mapDrawer.GetMapScaler();

        startPosition = transform.position;
        agroRange = 4 * mapk;
        speed = defaultSpeed;
        faceTo = "Right";
        stamina = maxStamina;
        health = maxHealth;
        isDied = false;
        enemyAttacks = new List<Attack>();

        CreateAttacksList();
        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }

    // Update is called once per frame
    private void Update()
    {
        if (isDied)
            return;

        if (!GM.isPaused)
        {
            if (!anim.isAttacking)
            {
                DefaultBehavior();
                //Move(target, speed);
            }
            //attackTime -= Time.deltaTime;
            //AnimPlay();
            RefreshStamina();
            //PlayerFind();
        }
    }

    void FixedUpdate()
    {
        if (isDied || GM.isPaused)
            return;

        PlayerFind();
        ChoosePath();
       
    }

    ////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////


    //////////////////// STARTING METHODS /////////////////////////
    //////////////////////////////////////////////////////////////

    private void CreateAttacksList()
    {
        int attacksCount = attackPoses.Count;

        for (int i = 0; i < attacksCount; i++)
        {
            string name = attackPoses[i].name.Substring(0, attackPoses[i].name.Length - 3);
            //Vector2 pos = attackPoses[i].position;
            float range = attackRanges[i];
            int damage = attackDamages[i];

            Attack attack = new Attack(name, range, damage);
            enemyAttacks.Add(attack);
            HitBox pos = attackPoses[i].GetComponent<HitBox>();
            pos.damage = damage;
            pos.owner = "Enemy";
        }

    }

    //////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////


    ///////////////////// UPDATING METHODS //////////////////////
    /////////////////////////////////////////////////////////////

    protected void RefreshStamina()
    {
        if (stamina < maxStamina)
        {
            if (anim.curState == "Idle")
                stamina += Time.deltaTime * 20;
            else if (anim.curState == "Run")
                stamina += Time.deltaTime * 10;
        }
    }

    public void SawPlayer(Vector2 playerPos)
    {
        currentPlayerPosition = playerPos;
        currentAgroTime = startAgroTime;
        sawPlayer = true;
        //Debug.Log("SawPlayer:" + sawPlayer + "cord: " + playerPos);
    }

    ////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////


    ///////////////////////// PATHFINDNG //////////////////////////
    //////////////////////////////////////////////////////////////

    private void ChoosePath()
    {
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

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rB.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        Move(force);

        float distance = Vector2.Distance(rB.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypiontDistance)
        {
            currentWaypoint++;
        }
    }

    private void UpdatePath()
    {
        if (seeker.IsDone())
            seeker.StartPath(rB.position, target, OnPathComplete);
    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
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

        if (sawPlayer)
        {
            target = currentPlayerPosition;
            currentAgroTime -= Time.deltaTime;
        }
        else
        {
            target = startPosition;
        }
    }

    /////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////


    ///////////////////////// ACTIONS ///////////////////////////////
    ////////////////////////////////////////////////////////////////

    protected void Move(Vector2 force)
    {
        if (Vector2.Distance(target, rB.position) > attackRadius)
        {
            //moveVelocity = (target - rB.position) * moveSpeed;
            if (rB.velocity.x <= -0.01f && faceTo == "Right")
            {
                faceTo = "Left";
                Flip();
            }
            else if (rB.velocity.x >= 0.01f && faceTo == "Left")
            {
                faceTo = "Right";
                Flip();
            }

            //rB.MovePosition(rB.position + moveVelocity * Time.deltaTime);

            rB.AddForce(force);
            //Flip();



            /*xCord = Mathf.FloorToInt(rB.position.x / mapk + 0.5f);
            yCord = Mathf.FloorToInt(rB.position.y / mapk + 0.5f);
            MapManager.map[xCord, yCord].hasEnemy = false;
            MapManager.map[xCord, yCord].enemy = null;
            rB.MovePosition(rB.position + moveVelocity * Time.deltaTime);
            //rB.position = Vector2.MoveTowards(rB.position, target, moveSpeed * Time.deltaTime);
            xCord = Mathf.FloorToInt(rB.position.x / mapk + 0.5f);
            yCord = Mathf.FloorToInt(rB.position.y / mapk + 0.5f);
            MapManager.map[xCord, yCord].hasEnemy = true;
            MapManager.map[xCord, yCord].enemy = this.gameObject;*/
        }
    }

    void Flip()
    {
        Vector2 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;

/*         Vector2 healthBarScaler = healthBar.transform.localScale;
        scaler.x *= -1;
        healthBarCanvas.transform.localScale = healthBarScaler; */
    }

    public void GetDamage(float damage)
    {
        ChangeHealthBar();
        StartCoroutine(Hurting());
        health -= damage;
        currentAgroTime = startAgroTime;

        if (health <= 0)
        {
            anim.curState = "Dying";
            isDied = true;
            enemyCollider.isTrigger = true;
            GM.EnemyDie();
            rB.constraints = RigidbodyConstraints2D.FreezePosition;
            healthBar.fillAmount = 0f;
            healthBarCanvas.SetActive(false);

            if(!isBig)
                sortGr.sortingLayerName = "Items";
            //Debug.Log(GM.aliveEnemies);
        }
    }

    protected virtual void DefaultBehavior() { }
    protected virtual void MakeAttack() {}

    ///////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////


    //////////////////////// COURUTINES ///////////////////////////////////
    //////////////////////////////////////////////////////////////////////

    protected IEnumerator Attacking()
    {
        anim.curAttack = curAttack;
        anim.isAttacking = true;
        yield return new WaitForSeconds(0.5f);
        anim.isAttacking = false;
    }

    protected IEnumerator Hurting()
    {
        anim.curState = "Hurt";
        anim.isHurting = true;
        yield return new WaitForSeconds(0.5f);
        anim.isHurting = false;
    }

    //////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////

    private void ChangeHealthBar() {
        healthBar.fillAmount = health / maxHealth;
    }

}
