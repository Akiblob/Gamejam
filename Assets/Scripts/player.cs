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
    private float timer;

    public int maxHealth = 100;
    int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();
    }

    void Update()
    {

        move();
        moveAttackPoint();
        flip();


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

    void flip()
    {
        if ((attackPoint.position - transform.position).x > 0) sprite.flipX = false;
        else sprite.flipX = true;
    }
    


    void attack()
    {
        StartCoroutine(playerAnimation());
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

    private IEnumerator playerAnimation()
    {
        anim.Play("attackAnimation");
        yield return new WaitForSeconds(0.5f);
        anim.Play("IdleAnimation");

        yield break;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if(currentHealth < 1)
        {
            DEATH();
        }
    }

    void DEATH()
    {

    }
   
}
