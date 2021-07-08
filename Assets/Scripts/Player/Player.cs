using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private DungeonGenerator dungeon;

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
        anim = GetComponent<PlayerAnimator>();

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
            {
                Move();
                rB.velocity = new Vector2(0, 0);
            }
        }
    }

    //////////////////������������� ������////////////////////

    public void Move()
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

            rB.MovePosition(rB.position + moveVelocity * Time.deltaTime);
            //prevX = Mathf.FloorToInt(rB.position.x / mapk + 0.5f);
            //prevY = Mathf.FloorToInt(rB.position.y / mapk + 0.5f);
            //MapManager.map[prevX, prevY].hasPlayer = false;
            //Debug.Log(MapManager.map[prevX, prevY].hasPlayer);
            
           //curX = Mathf.FloorToInt(rB.position.x / mapk + 0.5f);
            //curY = Mathf.FloorToInt(rB.position.y / mapk + 0.5f);
            //MapManager.map[curX, curY].hasPlayer = true;
            //Debug.Log(MapManager.map[curX, curY].hasPlayer);
        }
        else
        {
            anim.isMoving = false;
        }
    }

    private void Actions()
    {
        if (Input.GetKeyDown("f") && doorScript != null)
        {
            doorScript.ChangeDoorState();
        }
    }

    public void Combat()
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
        rB.isKinematic = true;
        isDied = true;
        anim.isDied = true;
        anim.isActing = true;
        anim.isMoving = false;
        anim.act = "Died";
        yield return new WaitForSeconds(0.3f);
        anim.act = "Die";
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(2);
    }

    /////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////


    /////////// COLLISION, COLLIDER TRIGGER, COLLIDE ///////////
    ////////////////////////////////////////////////////////////

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ConnectWithDoor(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        ConnectWithDoor(collision);
    }

    private void OnTriggerExit2D()
    {
        doorScript = null;
    }

    ////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////


    ////////////////�������������� � �����������////////////////

    private void StatBarChange()
    {
        staminaBar.fillAmount = stamina.curValue / stamina.curMaxValue;
        healthBar.fillAmount = health.curValue / health.curMaxValue;
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
        if (health.curValue < health.curMaxValue)
        {
            if (health.curValue + healthIncr > health.curMaxValue)
                health.curValue += health.curMaxValue - health.curValue;
            else
                health.curValue += healthIncr;
        }
    }

    public void GetContiniousEffect(float increase, float time, PotionType effect)
    {
        switch (effect)
        {
            case PotionType.strength:
                if (strength.timeEffect != 0)
                {
                    strength.timeEffect = time;
                }
                else
                {
                    strength.curMaxValue += increase;
                    strength.curValue += increase;
                    strength.timeEffect = time;
                    RefreshHitBoxDamage();
                }
                break;
            case PotionType.stamina:
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
            case PotionType.speed:
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

    public void GetDamage(float damage)
    {
        if (isDied)
            return;

        if (isDefending)
        {
            float staminaSub = damage * 10;
            if (staminaSub > stamina.curValue)
            {
                stamina.curValue = 0;
                staminaSub -= stamina.curValue;
                health.curValue -= staminaSub / 10;
            }
            else
                stamina.curValue -= staminaSub;
        }
        else
            health.curValue -= damage;

        if(health.curValue <= 0)
            StartCoroutine(Dying());
        else
            StartCoroutine(Hurting());
    }

    private void RefreshHitBoxDamage()
    {
        damage = strength.curValue + weaponDamage;
        foreach (var hitBox in hitBoxes)
        {
            HitBox hB = hitBox.GetComponent<HitBox>();
            hB.damage = damage;
            hB.thrust = strength.curValue * 50000;
            hB.owner = "Player";
        }
    }


    ///////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////




    ////////////////��������� ��������������///////////////////////

    private void ConnectWithDoor(Collider2D collision)
    {
        if (collision.GetComponent<DoorOpening>())
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
