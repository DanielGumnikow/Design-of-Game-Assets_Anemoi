using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private LayerMask platformLayerMask;

    public SkeletonAnimation skeletonAnimation;
    public AnimationReferenceAsset idle, walking, floating, running, jump, jumpfloat;
    public string currentState;
    public string currentAnimation;

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

    public float speed;
    public float movementx;

    private float periodLength = 10; //Seconds
    private float amount = 10; //Amount to add

    void Start()
    {
        dashTime = startDashTime;
        currentState = "Idle";
        SetCharacterState(currentState);
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
            movementx = Input.GetAxis("Horizontal");
            rb.velocity = new Vector2(movementx * speed, rb.velocity.y);

            if (movementx != 0)
            {
                SetCharacterState("Walking");
                if (movementx > 0)
                {
                    transform.localScale = new Vector2(0.5f, 0.5f);
                }
                else
                {
                    transform.localScale = new Vector2(-0.5f, 0.5f);
                }
            }
            else
            {
                SetCharacterState("Idle");
            }
            /*
            Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0f, 0f);
            transform.position += movement * Time.deltaTime * moveSpeed;
            Physics.gravity = new Vector3(0, 0.0F, 0);
            */

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

    public void SetAnimation(AnimationReferenceAsset animation, bool loop,float timeScale)
    {
        if (animation.name.Equals(currentAnimation))
        {
            return;
        }
        skeletonAnimation.state.SetAnimation(0, animation, loop).TimeScale = timeScale;
        currentAnimation = animation.name;
    }

    public void SetCharacterState(string state)
    {
        if (state.Equals("Idle"))
        {
            SetAnimation(idle, true, 1f);
        }
        else if(state.Equals("Walking"))
        {
            SetAnimation(walking, true, 0.2f);
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
        float extraHeightText = 1f;
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