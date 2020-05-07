using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private LayerMask platformLayerMask;

    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private BoxCollider2D bc;

    public static float MaxJumpValue = 100;
    public static float JumpValue;

    public static float MaxDashValue = 2;
    public static float DashValue;

    public static float MaxFloatValue = 100;
    public static float FloatValue;

    public static bool controllable = true;
    public static bool abilities = true;

    public float dashSpeed;
    private float dashTime;
    public float startDashTime;
    private int direction;

    float coolingdownCounter = 4;
    bool coolingdown = false;

    private float periodLength = 10; //Seconds
    private float amount = 10; //Amount to add

    void Start()
    {
        dashTime = startDashTime;
    }

    private void Awake()
    {
        Infoscript curr = Infoscript.instance;
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        //Debug.Log("controllable" + controllable);
        //Debug.Log("abilities" + abilities);
        if (controllable == true) {
            Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0f, 0f);
            transform.position += movement * Time.deltaTime * moveSpeed;
            Physics.gravity = new Vector3(0, 0.0F, 0);

            if (abilities == true) { 
            Dash();
            Jump();
            Floating();
            }
        }
        if (Input.GetMouseButtonDown(1)) {
            //Player.HealthPoints -= 1;
            Infoscript.instance.DamageHealthpoints(1);
        }
    } 

    void Dash() {
        if (direction == 0)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                //Instantiate(dashEffect, transform.position, Quaternion.identity);
                direction = 1;
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                //Instantiate(dashEffect, transform.position, Quaternion.identity);
                direction = 2;
            }
        }
        else {
            if (dashTime <= 0)
            {
                direction = 0;
                dashTime = startDashTime;
                rb.velocity = Vector2.zero;
            }
            else {
                dashTime -= Time.deltaTime;

                if (direction == 1 && DashValue >= 1)
                {
                    //Infoscript.instance.UseDash(1);
                    rb.velocity = Vector2.up * dashSpeed;
                }
                else if (direction == 2 && DashValue >= 1)
                {
                    //Infoscript.instance.UseDash(1);
                    rb.velocity = Vector2.right * dashSpeed;
                }         
            }
        }
    }

    void Jump()
    {
        if (isGrounded() && Input.GetButtonDown("Jump"))
        {
            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, ((JumpValue*0.01f)* 10f)), ForceMode2D.Impulse);
            //Infoscript.instance.UseStamina(JumpValue);
            //Infoscript.instance.Use_Stamina(JumpValue, JumpValue, Infoscript.instance.staminaBar, Infoscript.instance.regen , Infoscript.instance.RegenStamina());

        }
    }

    void Floating()
        {
        if (Input.GetButton("Fire1"))
        {
            if(FloatValue > 0) { 
            //Infoscript.instance.UseFloat(0.2f);
            rb.gravityScale = 0.1f;
            }
            if (FloatValue < 1)
            {
                FloatValue = 0;
                rb.gravityScale = 0.8f;
            }
            }
        else {rb.gravityScale = 0.8f; }
        }

    private bool canMove(Vector3 dir, float distance) {
        return Physics2D.Raycast(transform.position, dir, distance).collider == null;
    }

    private bool isGrounded() {
        float extraHeightText = 0.03f;
        RaycastHit2D raycastHit = Physics2D.Raycast(bc.bounds.center, Vector2.down, bc.bounds.extents.y + extraHeightText, platformLayerMask);
        Color rayColor;
        if (raycastHit.collider != null)
        {
            rayColor = Color.green;
        }
        else {
            rayColor = Color.yellow;
        }
        Debug.DrawRay(bc.bounds.center, Vector2.down * (bc.bounds.extents.y + extraHeightText), rayColor);
        return raycastHit.collider != null;
    }
}