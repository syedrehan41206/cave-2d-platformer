using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
using JetBrains.Annotations;

public class PlayerControl : MonoBehaviour
{
    public Rigidbody2D rb;
    public InputManager controller;
    public float speed = 5f;
    [SerializeField] float movespeed;
    float speedMultiplyer = 1f;
    public float speedboostmulti = 1.5f;
    public float jumpforce = 1f;
    public LayerMask Groundlayer;
    public Transform GroundCheck;
    public LayerMask Wall;
    private bool hit;
    public float maxHealth = 10f;
    public float currentHealth;
    public bool candash;
    public float dashDuration;
    public float dashSpeed;
    float Gravity;
    public Animator Anim;
    public SpriteRenderer SpriteRenderer;

    private void Start()
    {
        Gravity = rb.gravityScale;
        Anim=GetComponent<Animator>();
    }
    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        Anim.SetTrigger("Damage");
        if (currentHealth <= 0)
        {
            Debug.Log("You Are Dead");
           
        }
    }
    public void Heal(float amount)
    {
        currentHealth += amount;
        Anim.SetTrigger("Heal");
        currentHealth = Mathf.Clamp(currentHealth,0f, maxHealth);
    }

    public bool isGrounded()
    {
        return Physics2D.OverlapCircle(GroundCheck.position, 0.1f, Groundlayer);
    }
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        controller = GetComponent<InputManager>();
        currentHealth = maxHealth;
        Anim = GetComponent<Animator>();
        SpriteRenderer = GetComponent<SpriteRenderer>();

    }
    void StartSpeedBoost(float multiplayer)
    {
        StartCoroutine(SpeedBoostCoroutine(multiplayer));
    }
    private IEnumerator SpeedBoostCoroutine(float multiplyer)
    {
        speedMultiplyer = multiplyer;
        Anim.SetBool("Boost", true);
        yield return new WaitForSeconds(2f);
        speedMultiplyer =1f;
        Anim.SetBool("Boost", false);
    }
    
    void Update()
       
    {
        
       if(controller.dashInput&&candash)
        {
            StartCoroutine(Dash(dashDuration, dashSpeed));
        }
        Move();
        if (isGrounded() && controller.JumpInput)
        {
            Jump();
        }
        hit = Physics2D.Raycast(transform.position, Vector2.up, 10f, Wall);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position,Vector2.up * 10f);
    }
    public void Move()
    {
        movespeed = speed * speedMultiplyer;
        rb.linearVelocityX = movespeed;
    }
    public void Jump()
    {
        rb.linearVelocityY = jumpforce;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Damage"))
        {
            TakeDamage(2f);
        }
        if(collision.gameObject.CompareTag("Speeditem"))
        {
            StartSpeedBoost(speedboostmulti);
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("Death"))
        {
            TakeDamage(maxHealth);
        }
        if (collision.gameObject.CompareTag("Heal"))
            Heal(25);
            Destroy(collision.gameObject);
    }
    public IEnumerator Dash(float dashDuration, float dashSpeed)
    {
        float elapsed = 0f;

        while (elapsed < dashDuration)
        {
            candash = false;
            rb.linearVelocity = dashSpeed*Vector2.right;
            elapsed += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(0.1f);
        candash = true;
    }
    public void GravityBegone()
    {
        Gravity *= -1;
        jumpforce *= -1;
        transform.localScale = new Vector3(transform.localScale.x,transform.localScale.y * -1,transform.localScale.z);
        rb.gravityScale = Gravity;
    }


} 