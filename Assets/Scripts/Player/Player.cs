using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private DungeonGenerator dungeon;

    //public Vector2Int playerPosition;

    private MapDrawer MD;
    private GameObject gameManager;
    private GameManager GM;

    public Image staminaBar;
    public Image healthBar;


    public List<GameObject> hitBoxes;
    public Attribute health;
    public Attribute stamina;
    public Attribute speed;
    public Attribute strength;
    //public Transform hitPos;
    //public float speed;
    //public float maxHealth;
    //public float maxStamina;
    public int noiseRange;
    public float hitRange;
    public float weaponDamage;
    public bool isDied;
    public float staminaPerSec;



    private GameObject staminaEffectBar;
    private GameObject strengthEffectBar;
    private GameObject speedEffectBar;
    private TextMeshProUGUI staminaTimer;
    private TextMeshProUGUI strengthTimer;
    private TextMeshProUGUI speedTimer;
    private Rigidbody2D rB;
    private PlayerAnimator anim;
    private GameObject door;
    private DoorOpening doorScript;
    private Vector2 inputMovement;
    private Vector2 moveVelocity;

    private float mapk;
    private int prevX;
    private int prevY;
    private int curX;
    private int curY;
    private int mapWidth;
    private int mapHeight;
    private float staminaPerAttack;
    private float defence;
    private bool isDefending;
    private float damage;



    public void Start()
    {
        rB = GetComponent<Rigidbody2D>();
        //anim = GetComponent<Animator>();
        anim = GetComponent<PlayerAnimator>();
        //canvasTransform = GameObject.Find("UI").transform;

        staminaBar = GameObject.FindGameObjectWithTag("StaminaBar").GetComponent<Image>();
        healthBar = GameObject.FindGameObjectWithTag("HealthBar").GetComponent<Image>();
        staminaEffectBar = GameObject.FindGameObjectWithTag("StaminaEffect");
        strengthEffectBar = GameObject.FindGameObjectWithTag("StrengthEffect");
        speedEffectBar = GameObject.FindGameObjectWithTag("SpeedEffect");
        staminaTimer = GameObject.FindGameObjectWithTag("StaminaTimer").GetComponent<TextMeshProUGUI>();
        strengthTimer = GameObject.FindGameObjectWithTag("StrengthTimer").GetComponent<TextMeshProUGUI>();
        speedTimer = GameObject.FindGameObjectWithTag("SpeedTimer").GetComponent<TextMeshProUGUI>();
        staminaBar.fillAmount = 1f;
        healthBar.fillAmount = 1f;

        gameManager = GameObject.Find("GameManager");

        GM = gameManager.GetComponent<GameManager>();
        MD = gameManager.GetComponent<MapDrawer>();
        dungeon = gameManager.GetComponent<DungeonGenerator>();

        mapk = MD.mapk;
        mapWidth = dungeon.mapWidth;
        mapHeight = dungeon.mapHeight;
        damage = strength.curValue + weaponDamage;
        defence = 1;
        staminaPerAttack = 30;
        isDied = false;


        RefreshHitBoxDamage();
        RefreshScripObj();
    }

    public void Update()
    {
        if(!GM.isPaused)
        {
            if (!isDied)
            {
                Combat();
                Actions();
                StaminaRefresh();
                EffectRefresh();
                NoiseFOVCheck(curX, curY);
            }

            StatBarChange();
            EffectBarChange();
        }
        
    }
    public void FixedUpdate()
    {
        if(!GM.isPaused)
        {
            if (!isDied)
                Move();
        }
        
    }

    //////////////////������������� ������////////////////////

    private void Move()
    {
        inputMovement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        moveVelocity = inputMovement * speed.curValue;
        if ((inputMovement.x != 0 || inputMovement.y != 0))
        {
            //anim.curState = "Run";
            anim.isMoving = true;
            anim.curState = "Run";

            if (inputMovement.y > 0)
                anim.faceTo = "Back";
            else if (inputMovement.y < 0)
                anim.faceTo = "Front";
            else if (inputMovement.x > 0)
                anim.faceTo = "Right";
            else if (inputMovement.x < 0)
                anim.faceTo = "Left";

            prevX = Mathf.FloorToInt(rB.position.x / mapk + 0.5f);
            prevY = Mathf.FloorToInt(rB.position.y / mapk + 0.5f);
            MapManager.map[prevX, prevY].hasPlayer = false;
            //Debug.Log(MapManager.map[prevX, prevY].hasPlayer);
            rB.MovePosition(rB.position + moveVelocity * Time.deltaTime);
            curX = Mathf.FloorToInt(rB.position.x / mapk + 0.5f);
            curY = Mathf.FloorToInt(rB.position.y / mapk + 0.5f);
            MapManager.map[curX, curY].hasPlayer = true;
            //Debug.Log(MapManager.map[curX, curY].hasPlayer);
        }
        else
        {
            anim.isMoving = false;
            //anim.curState = "Idle";
            /*anim.SetBool("runFrontA1", false);
            anim.SetBool("runBackA1", false);
            anim.SetBool("runLeftA1", false);
            anim.SetBool("runRightA1", false);*/

        }
    }

    private void Actions()
    {
        if (Input.GetKeyDown("f") && doorScript != null)
        {
            doorScript.ChangeDoorState();
        }
    }

    private void Combat()
    {
        if (Input.GetMouseButtonDown(0) && stamina.curValue >= staminaPerAttack && anim.isActing == false)
        {
            stamina.curValue -= 30;
            StartCoroutine(Attacking());
        }


        if (Input.GetMouseButtonDown(1) && anim.isActing == false)
        {
            isDefending = true;
            StartCoroutine(Blocking());

            /*int x = Mathf.FloorToInt(rB.position.x / mapk + 0.5f);
            int y = Mathf.FloorToInt(rB.position.y / mapk + 0.5f);

            for (int i = x - 4; i < x + 4; i++)
            {
                for (int j = y - 4; j < y + 4; j++)
                {
                    Debug.Log(MapManager.map[x, y].hasPlayer);
                }
            }*/
        }

        if (Input.GetMouseButton(1))
        {
            anim.isActing = true;
            anim.act = "Blocking";
        }

        if (Input.GetMouseButtonUp(1))
        {
            anim.isActing = false;
            isDefending = false;
            /*anim.isBlocking = false;
            anim.curState = "Idle";
            defence = 1;
            anim.animIsBlocked = false;*/
        }

        /*if(Input.GetMouseButtonDown(2))
        {
            StartCoroutine(Dying());
        }*/

        if (Input.GetKeyDown("j"))
        {
            GetDamage(damage);
        }

        if(Input.GetKeyDown("h"))
        {
            health.curValue = health.curMaxValue;
        }
    }
    ////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////




    ////////////////////////��������////////////////////////////////

    private IEnumerator Attacking()
    {
        anim.isActing = true;
        anim.act = "Attack_1";
        yield return new WaitForSeconds(0.4f);
        anim.isActing = false;
    }

    private IEnumerator Blocking()
    {
        Debug.Log("����");
        anim.isActing = true;
        anim.act = "Block";
        yield return new WaitForSeconds(0.5f);
        anim.act = "Blocking";
    }

    private IEnumerator Hurting()
    {
        anim.isActing = true;
        anim.act = "Hurt";
        yield return new WaitForSeconds(0.5f);
        anim.act = "";
        anim.isActing = false;
    }

    private IEnumerator Dying()
    {
        isDied = true;
        anim.isActing = true;
        anim.isMoving = false;
        anim.act = "Died";
        yield return new WaitForSeconds(0.5f);
        anim.act = "Die";
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(2);
    }
    /////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////


    ////////////////�������������� � �����������////////////////

    private void StatBarChange()
    {
        staminaBar.fillAmount = stamina.curValue / stamina.curMaxValue;
        healthBar.fillAmount = health.curValue / health.curMaxValue;
        //Debug.Log(staminaBar.fillAmount);
    }

    private void EffectBarChange()
    {
        if (stamina.timeEffect != 0)
        {
            float time = stamina.timeEffect * 60;
            float minutes = Mathf.Floor(time / 60);
            staminaEffectBar.SetActive(true);
            staminaTimer.text = $"{Mathf.Floor(minutes)} : {Mathf.Round(time - (minutes * 60))}";

        }
        else
            staminaEffectBar.SetActive(false);

        if (strength.timeEffect != 0)
        {
            float time = strength.timeEffect * 60;
            float minutes = Mathf.Floor(time / 60);
            strengthEffectBar.SetActive(true);
            strengthTimer.text = $"{Mathf.Floor(minutes)} : {Mathf.Round(time - (minutes * 60))}";
        }

        else
            strengthEffectBar.SetActive(false);

        if (speed.timeEffect != 0)
        {
            float time = speed.timeEffect * 60;
            float minutes = Mathf.Floor(time / 60);
            speedEffectBar.SetActive(true);
            speedTimer.text = $"{Mathf.Floor(minutes)} : {Mathf.Round(time - (minutes * 60))}";

        }
        else
            speedEffectBar.SetActive(false);

    }

    //////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////



    ///////////////// ������ � ����������� ///////////////////////
    private void StaminaRefresh()
    {
        if (stamina.curValue < stamina.curMaxValue)
        {
            if (anim.curState == "Idle")
                stamina.curValue += Time.deltaTime * staminaPerSec * 2;
            else if (anim.curState == "Run")
                stamina.curValue += Time.deltaTime * staminaPerSec;
        }

    }

    private void EffectRefresh()
    {
        if (stamina.timeEffect < 0)
        {
            stamina.timeEffect = 0;
            stamina.curMaxValue = stamina.defaultMaxValue;
            staminaPerSec /= 2;
        }else if (stamina.timeEffect != 0)
            stamina.timeEffect -= 0.01f * Time.deltaTime;

        if (strength.timeEffect < 0)
        {
            strength.timeEffect = 0;
            strength.curMaxValue = strength.defaultMaxValue;
            strength.curValue = strength.curMaxValue;
            RefreshHitBoxDamage();
        }else if (strength.timeEffect != 0)
            strength.timeEffect -= 0.01f * Time.deltaTime;

        if (speed.timeEffect < 0)
        {
            speed.timeEffect = 0;
            speed.curMaxValue = speed.defaultMaxValue;
            speed.curValue = speed.curMaxValue;
        }else if (speed.timeEffect != 0)
            speed.timeEffect -= 0.01f * Time.deltaTime;
    }

    public void UpdateHealth(float healthIncr)
    {
        health.curValue += healthIncr;
    }

    public void GetContiniousEffect(float increase, float time, typeEnum effect)
    {
        switch (effect)
        {
            case typeEnum.strength:
                if (strength.timeEffect != 0)
                {
                    strength.timeEffect = time;
                }
                else
                {
                    Debug.Log(strength.curValue);
                    strength.curMaxValue += increase;
                    strength.curValue += increase;
                    strength.timeEffect = time;
                    RefreshHitBoxDamage();
                    Debug.Log(strength.curValue);
                    Debug.Log(increase);
                }
                break;
            case typeEnum.stamina:
                if (stamina.timeEffect != 0)
                {
                    stamina.timeEffect = time;
                }
                else
                {
                    stamina.curMaxValue *= increase;
                    stamina.timeEffect = time;
                    staminaPerSec *= 2;
                }
                break;
            case typeEnum.speed:
                if (speed.timeEffect != 0)
                {
                    speed.timeEffect = time;
                }
                else
                {
                    speed.curMaxValue *= increase;
                    speed.curValue *= increase;
                    speed.timeEffect = time;
                }
                break;
        }
    }

    //������� ���������� ���������� ��������

    /*public void ChangeSpeed(float speedMultiplier)
    {
        speed.curValue *= speedMultiplier;
    }*/

    public void GetDamage(float damage)
    {
        if (isDied)
            return;

        if (isDefending)
        {
            // Debug.Log("����� ����������");
            float staminaSub = damage * 10;
            if (staminaSub > stamina.curValue)
            {
                stamina.curValue = 0;
                staminaSub -= stamina.curValue;
                health.curValue -= staminaSub / 10;
            }
            else
                stamina.curValue -= staminaSub;
            // Debug.Log(stamina);
        }
        else
            health.curValue -= damage;

        /*if(health.curValue <= 0)
            StartCoroutine(Dying());
        else*/
            StartCoroutine(Hurting());

    }

    private void RefreshHitBoxDamage()
    {
        Debug.Log(strength.curValue);
        damage = strength.curValue + weaponDamage;
        foreach (var hitBox in hitBoxes)
        {
            HitBox hB = hitBox.GetComponent<HitBox>();
            hB.damage = damage;
            /*Debug.Log(damage);
            Debug.Log(strength.curValue);
            Debug.Log(weaponDamage);*/
            hB.thrust = strength.curValue * 50000;
            hB.owner = "Player";
        }
    }


    ///////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////




    ////////////////��������� ��������������///////////////////////
    void NoiseFOVCheck(int x, int y)
    {
        for (int i = -4; i <= 4; i++)
        {
            for (int j = -4; j <= 4; j++)
            {
                if (x + i < 0 || x + i > mapWidth)
                    break;
                if (y + j < 0 || y + j > mapHeight)
                    continue;

                int xCord = x + i;
                int yCord = y + j;
                try
                {
                    if (MapManager.map[xCord, yCord].hasEnemy == true)
                    {
                        Enemy enemy = MapManager.map[xCord, yCord].enemy.GetComponent<Enemy>();
                        enemy.SawPlayer(transform.position);
                        //enemy.sawPlayer = true;
                        //enemy.currentPlayerPosition = transform.position;
                        //enemy.savedPlayerPosition = transform.position;
                        //Debug.Log("��� ����");

                        //Debug.Log(MapManager.map[xCord, yCord].enemy.name);
                    }
                }
                catch (System.Exception)
                {
                    continue;
                }
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Door")
        {
            door = collision.gameObject;
            doorScript = door.GetComponent<DoorOpening>();
        }
    }

    //////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////

    //////////////////// ������ ��� ������������ /////////////////////

    private void RefreshScripObj()
    {
        health.curMaxValue = health.defaultMaxValue;
        health.curValue = health.curMaxValue;
        health.timeEffect = 0;

        speed.curMaxValue = speed.defaultMaxValue;
        speed.curValue = speed.curMaxValue;
        speed.timeEffect = 0;

        stamina.curMaxValue = stamina.defaultMaxValue;
        stamina.curValue = stamina.curMaxValue;
        stamina.timeEffect = 0;

        strength.curMaxValue = strength.defaultMaxValue;
        strength.curValue = strength.curMaxValue;
        strength.timeEffect = 0;
    }
}