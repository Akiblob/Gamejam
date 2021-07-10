using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    
    public float attackRange = 1f;
    public int damage = 10;
    public float speed;
    public LayerMask enemyLayers;
    public Rigidbody2D rb;
    Vector2 movement;

    public float attackRate = 2f;
    float nextAttackTime = 0f;

    public Transform attackPoint;
    public Transform equipPoint;

    public SpriteRenderer sprite;

    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        move();
        moveAttackPoint();

        if(Input.GetAxisRaw("Horizontal") < 0)
        {
            sprite.flipX = true;
        }
        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            sprite.flipX = false;
        }


        if (Time.time >= nextAttackTime)
        {
            if (Input.GetMouseButtonDown(0))
            {
                attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
        
    }


    void FixedUpdate()
    {
        //move player
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
        
    }


    void move()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
    }


    void attack()
    {

        anim.SetBool("Attacking", true);

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        
        foreach (Collider2D enemy in hitEnemies)
        {

            enemy.GetComponent<enemy>().TakeDamage(damage);
        }

        transform.position = Vector3.MoveTowards(transform.position, attackPoint.position, Time.deltaTime * 500);
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    void moveAttackPoint()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector3 dir = (mousePos - transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        equipPoint.eulerAngles = new Vector3(0, 0, angle);

    }
}
