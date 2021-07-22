using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour, IGetDamage, IGetEffect, IKnockbackable
{
    private DungeonGenerator dungeon;
    private EffectBarUI effectBarUI;

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
    private bool isInvencible;
    private bool isKnockbacked;



    public void Start()
    {
        rB = GetComponent<Rigidbody2D>();
        anim = GetComponent<PlayerAnimator>();

        staminaBar = GameObject.FindGameObjectWithTag("StaminaBar").GetComponent<Image>();
        healthBar = GameObject.FindGameObjectWithTag("HealthBar").GetComponent<Image>();
        staminaBar.fillAmount = 1f;
        healthBar.fillAmount = 1f;

        gameManager = GameObject.Find("GameManager");
        effectBarUI = GameObject.FindGameObjectWithTag("EffectBar").GetComponent<EffectBarUI>();
        effectBarUI.GetPlayer(this);

        GM = gameManager.GetComponent<GameManager>();
        MD = gameManager.GetComponent<MapDrawer>();
        dungeon = gameManager.GetComponent<DungeonGenerator>();

        damage = strength.curValue + weaponDamage;
        defence = 1;
        staminaPerAttack = 30;
        isDied = false;


        RefreshScripObj();
        RefreshHitBoxDamage();
    }

    public void Update()
    {
        if(!GM.isPaused)
        {
            if (!isDied)
            {
                Actions();
                StaminaRefresh();
                //EffectRefresh();
            }

            StatBarChange();
            //EffectBarChange();
        }
        
    }
    public void FixedUpdate() {
        if(GM.isPaused || isDied || isKnockbacked) {
            return;
        }
        
        rB.velocity = new Vector2(0, 0);
    }

    //////////////////������������� ������////////////////////

    public void Move(Vector2 inputMovement) {
        if(isDied) { 
            return;
        }
        //inputMovement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        moveVelocity = inputMovement * speed.curValue;
        if ((inputMovement.x != 0 || inputMovement.y != 0))
        {
            anim.SetState(true, "Run");

            if (inputMovement.y > 0)
                anim.ChangeFaceTo(PlayerFaceTo.back);
            else if (inputMovement.y < 0)
                anim.ChangeFaceTo(PlayerFaceTo.front);
            else if (inputMovement.x > 0)
                anim.ChangeFaceTo(PlayerFaceTo.right);
            else if (inputMovement.x < 0)
                anim.ChangeFaceTo(PlayerFaceTo.left);

            rB.MovePosition(rB.position + moveVelocity * Time.deltaTime);
        }
        else
        {
            anim.SetState(false);
        }
    }

    private void Actions()
    {
        if (Input.GetKeyDown("f") && doorScript != null)
        {
            doorScript.ChangeDoorState();
        }
    }

    public void Attack() {
        if(stamina.curValue >= staminaPerAttack && anim.isActing == false && !isDied) {
            stamina.curValue -= 30;
            anim.SetActing("Attack");
        }
    }
    public void Blocking() {
        isDefending = true;
        anim.SetAdditionalState(true, "Blocking");
    }
    public void EndBlock() {
        isDefending = false;
        anim.SetAdditionalState(false);
    }

    public void ThrowAmmo(Vector2 vector, GameObject ammo) {
        GameObject instAmmo;
        Vector2 distanceNorm = (vector - rB.position).normalized;
        
        ammo.GetComponent<Item>().SetCount(1);
        ChangeVieweDirection(distanceNorm);

        instAmmo = Instantiate(ammo, rB.position, Quaternion.identity);
        instAmmo.GetComponent<ThrowingItem>().Launch(distanceNorm);
    }

    private void ChangeVieweDirection(Vector2 vector) {
        float num = Mathf.Abs(vector.x) > Mathf.Abs(vector.y) ? vector.x : vector.y; 
        string side = Mathf.Abs(vector.x) > Mathf.Abs(vector.y) ? "x" : "y";

        if(side == "x") {
            if(num > 0) {
                anim.ChangeFaceTo(PlayerFaceTo.right);
            } else {
                anim.ChangeFaceTo(PlayerFaceTo.left);
            }
        } else {
            if(num > 0) {
                anim.ChangeFaceTo(PlayerFaceTo.back);
            } else {
                anim.ChangeFaceTo(PlayerFaceTo.front);
            }
        }
    }
    ////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////




    ////////////////////////��������////////////////////////////////

    private IEnumerator BlockingCorutine()
    {
        anim.SetActing("Block");
        yield return new WaitForSeconds(0.5f);
        anim.SetAdditionalState(true, "Blocking");
    }

    private IEnumerator Dying()
    {
        rB.isKinematic = true;
        isDied = true;
        anim.PlayerDie();
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(2);
    }
    private IEnumerator InvencibleCoroutine() {
        isInvencible = true;
        yield return new WaitForSeconds(0.3f);
        isInvencible = false;
    }
     private IEnumerator KnockbackCoroutine() {
        isKnockbacked = true;
        yield return new WaitForSeconds(0.5f);
        isKnockbacked = false;
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

    public void GetIncrease(AttributeType attributeType, float increase) {
        Attribute curAttribute = health;

        switch(attributeType) {
            case AttributeType.health:
                curAttribute = health;
                break;
            case AttributeType.stamina:
                curAttribute = stamina;
                break;
            case AttributeType.strength:
                curAttribute = strength;
                break;
            case AttributeType.speed:
                curAttribute = speed;
                break;
        }

        if (curAttribute.curValue < curAttribute.curMaxValue)
        {
            if (health.curValue + increase > health.curMaxValue)
                health.curValue += health.curMaxValue - health.curValue;
            else
                health.curValue += increase;
        }
    }

    public void GetEffect(EffectClass effectClass, EffectType effectType, float increase, float time) {
        Dictionary<string, object> effectInfo = EffectDataBase.GetEffectInformation(effectClass, effectType);
        Effect curEffect  = (Effect)effectInfo["Effect"];
        Attribute varAttribute = health;

        switch(effectType) {
            case EffectType.maxStamina:
                varAttribute = stamina;
                break;
            case EffectType.currentSpeed:
                varAttribute = speed;
                break;
            case EffectType.currentStrength: 
                varAttribute = strength;
                break;
        }

        curEffect.SetAttribute(varAttribute);
        curEffect.SetIncrease(increase);
        //varAttribute.GetEffect(valueType, increase, time);
        effectBarUI.SetEffect(effectClass, effectType, time);

        if(effectType == EffectType.currentStrength) {
            RefreshHitBoxDamage();
        }
    }

    public void UnsetEffect(EffectClass effectClass, EffectType effectType) {
        Dictionary<string, object> effectInfo = EffectDataBase.GetEffectInformation(effectClass, effectType);
        Effect curEffect = (Effect)effectInfo["Effect"];

        Debug.Log("EndEffect");
        curEffect.EndEffect();

        if(effectType == EffectType.currentStrength) {
            RefreshHitBoxDamage();
        }
    }

    public void GetDamage(float damage)
    {
        if (isDied || isInvencible)
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

        /* if(health.curValue <= 0)
            StartCoroutine(Dying());
        else */
            anim.SetActing("Hurt");
            StartCoroutine(InvencibleCoroutine());
    }

    private void RefreshHitBoxDamage() {
        Debug.Log(strength.curValue);
        damage = strength.curValue + weaponDamage;

        foreach (var hitBox in hitBoxes) {
            HitBox hB = hitBox.GetComponent<HitBox>();
            hB.damage = damage;
            hB.thrust = strength.curValue;
            hB.owner = "Player";
            Debug.Log(hB.damage);
        }
    }

    public void Knockback(Vector2 knockVector, float power) {
        //rB.MovePosition(rB.position + knockVector * 4);
        rB.AddForce(knockVector * 0.4f);
        StartCoroutine(KnockbackCoroutine());
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
