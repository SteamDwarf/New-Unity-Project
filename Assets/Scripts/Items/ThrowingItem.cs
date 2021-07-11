using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingItem : Item
{
    [SerializeField] protected float damage;
    [SerializeField] protected float power;
    [SerializeField] protected ItemDamageType type;
    protected Rigidbody2D rb;
    protected Player player;
    protected Animator anim;
    protected bool isLaunched;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void UseItem() {}

    protected override void OnTriggerEnter2D(Collider2D other) {
        base.OnTriggerEnter2D(other);

        if(other.GetComponent<Enemy>() != null && isLaunched) {
            Enemy enemy = other.GetComponent<Enemy>();

            if(!enemy.isDied) {
                other.GetComponent<Enemy>().GetDamage(damage);
                Destroy(this.gameObject);
            }

        } else if(other.CompareTag("Wall")) {
            Destroy(this.gameObject);
        }
        
    }

    public void Launch(Vector2 target) {
        if(rb == null) {
            rb = GetComponent<Rigidbody2D>();
        }
        if(anim == null) {
            anim = GetComponent<Animator>();
        }

        rb.AddForce(target * power);
        anim.Play("Launched");
        StartCoroutine(LaunchCorutine());
    }

    IEnumerator LaunchCorutine() {
        isLaunched = true;
        yield return new WaitForSeconds(2f);
        rb.velocity = new Vector2(0, 0);
        anim.Play("Droped");
        isLaunched = false;
    }
}
