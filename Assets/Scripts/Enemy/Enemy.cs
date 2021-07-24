using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Enemy : MonoBehaviour, IGetDamage, IKnockbackable
{
    protected MapDrawer mapDrawer;
    protected GameObject gameManager;
    protected Collider2D enemyCollider;
    protected EnemyAnimator anim;

    //[SerializeField] protected GameObject effectBar;
    //[SerializeField] protected GameObject effectPrefab;
    [SerializeField] protected GameObject healthBarCanvas;
    [SerializeField] protected Image healthBar;
    //[SerializeField] protected Dictionary<EffectType, GameObject> gettedEffects;
    [SerializeField] protected List<GameObject> attackPoses;
    [SerializeField] protected List<float> attackRanges;
    [SerializeField] protected List<int> attackDamages;
    [SerializeField] protected float agroRange;

    [SerializeField] protected float defaultSpeed;
    [SerializeField] protected float maxHealth;
    [SerializeField] protected bool sawPlayer;
    [SerializeField] protected float startAgroTime;
    [SerializeField] protected int maxStamina;
    [SerializeField] protected float attackRadius;
    [SerializeField] protected bool isBig;
    [SerializeField] protected float nextWaypiontDistance;

    [SerializeField] private AudioClip hurtAudio;

    public bool isDied {get; protected set;}

    protected Rigidbody2D rB;
    protected Vector2 currentPlayerPosition;
    protected Vector2 moveVelocity;
    protected Vector2 startPosition;
    protected Vector2 target;
    protected List<Attack> enemyAttacks;
    protected GameManager GM;
    protected GameObject player;
    protected Rigidbody2D playerRB;
    protected Path path;
    protected Seeker seeker;
    protected SortingGroup sortGr;
    protected AudioPlayer audioPlayer;

    protected int xCord;
    protected int yCord;
    protected string faceTo;
    protected float mapk;
    //protected string enemyName;
    protected float currentAgroTime;
    protected float speed;
    protected float health;
    protected float stamina;
    //protected float attackTime;
    protected string curAttack;
    protected int currentWaypoint = 0;
    protected bool reachedEndOfPath = false;
    protected bool isAttacking;
    private bool isInvencible;

    /////////Start, Update, FixedUpdate//////////////////////

    protected virtual void Start()
    {
        gameManager = GameObject.Find("GameManager");
        player = GameObject.FindGameObjectWithTag("Player");
        playerRB = player.GetComponent<Rigidbody2D>();
        rB = GetComponent<Rigidbody2D>();
        anim = GetComponent<EnemyAnimator>();
        seeker = GetComponent<Seeker>();
        GM = gameManager.GetComponent<GameManager>();
        mapDrawer = gameManager.GetComponent<MapDrawer>();
        enemyCollider = gameObject.GetComponent<Collider2D>();
        sortGr = GetComponent<SortingGroup>();
        audioPlayer = GetComponent<AudioPlayer>();
        mapk = mapDrawer.GetMapScaler();

        startPosition = transform.position;
        agroRange = 4 * mapk;
        speed = defaultSpeed;
        faceTo = "Right";
        stamina = maxStamina;
        health = maxHealth;
        isDied = false;
        enemyAttacks = new List<Attack>();

       /*  gettedEffects = new Dictionary<EffectType, GameObject>() {
            {EffectType.currentSpeed, null},
            {EffectType.maxStamina, null}
        }; */

        CreateAttacksList();
        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }

    protected virtual void Update()
    {
        if (isDied)
            return;

        if (!GM.isPaused){
            if (!anim.isActing){
                DefaultBehavior();
            }
            RefreshStamina();
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
        if (Vector3.Distance(playerRB.position, this.gameObject.transform.position) < agroRange)
        {
            currentPlayerPosition = playerRB.position;
            currentAgroTime = startAgroTime;
            sawPlayer = true;
        } else {
            currentAgroTime = 0;
            sawPlayer = false;
        }
    }

    /////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////


    ///////////////////////// ACTIONS ///////////////////////////////
    ////////////////////////////////////////////////////////////////

    protected void Move(Vector2 force) {
        if (Vector2.Distance(target, rB.position) > attackRadius) {
            Vector2 vectorNorm = (target - rB.position).normalized;

            Flip(vectorNorm);
            rB.AddForce(force);
        }
    }

    protected void Flip(Vector2 vectorNorm) {
        if(vectorNorm.x <= -0.001f && faceTo == "Left") {
            return;
        }
        if(vectorNorm.x >= 0.001f && faceTo == "Right") {
            return;
        }

        if(faceTo == "Right") {
            faceTo = "Left";
        } else if(faceTo == "Left") {
            faceTo = "Right";
        }

        Vector2 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    public void GetDamage(float damage)
    {
        if(isDied || isInvencible) {
            return;
        }

        anim.SetActing("Hurt");
        audioPlayer.PlayOneShot(hurtAudio);

        health -= damage;
        currentAgroTime = startAgroTime;

        ChangeHealthBar();
        StartCoroutine(InvencibleCoroutine());

        if (health <= 0) {
            anim.CreatureDie();
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

    public void Knockback(Vector2 knockVector, float power) {
        rB.AddForce(knockVector * power);
    }

    //НЕ ЗАБУДЬ УНАСЛЕДОВАТЬ ОТ IGetEffect
    
    /* public void GetEffect(EffectClass effectClass, EffectType effectType, float increase, float time = 0) {
        if(gettedEffects[effectType] != null) {
            return;
        }

        Debug.Log("I Get Effect");
        Debug.Log(this.gameObject);
        GameObject instEffect;
        Dictionary<string, object> effectInfo = EffectDataBase.GetEffectInformation(effectClass, effectType);

        Debug.Log(speed);
        switch(effectType) {
            case EffectType.maxStamina:
                stamina *= increase;
                break;
            case EffectType.currentSpeed:
                speed *= increase;
                break;
        }

        Debug.Log(speed);
        instEffect = Instantiate(effectPrefab, Vector2.zero, Quaternion.identity, effectBar.transform);
        instEffect.GetComponent<Image>().sprite = Resources.Load<Sprite>((string)effectInfo["Image"]);
        gettedEffects[effectType] = instEffect;
    } */
    protected virtual void DefaultBehavior() { }
    protected virtual void MakeAttack() {}

    ///////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////


    //////////////////////// COURUTINES ///////////////////////////////////
    //////////////////////////////////////////////////////////////////////

        private IEnumerator InvencibleCoroutine() {
        isInvencible = true;
        yield return new WaitForSeconds(0.3f);
        isInvencible = false;
    }

    //////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////

    private void ChangeHealthBar() {
        healthBar.fillAmount = health / maxHealth;
    }

}
