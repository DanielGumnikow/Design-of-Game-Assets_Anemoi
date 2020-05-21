using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.Audio;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private LayerMask platformLayerMask;

    public SkeletonAnimation skeletonAnimation;
    public AnimationReferenceAsset idle, walking, floating, running, jumping, jumpfloat;
    public string currentState;
    public string previousState;
    public string currentAnimation;

    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private BoxCollider2D bc;

    public static float MaxJumpValue = 100;
    public static float JumpValue;

    public static float MaxDashValue = 2;
    public static float DashValue;

    private bool isDashButtonDownE = false;
    private bool isDashButtonDownW = false;

    private GameObject pFloat;
    private GameObject tempObject;

    private WaitForSeconds regenTickDash = new WaitForSeconds(0.02f);
    public Coroutine regenDash;

    public static float MaxFloatValue = 100;
    public static float FloatValue;

    public static bool controllable = true;
    public static bool abilities = true;

    private float dashAmount = 1f;
    public float dashSpeed;
    private float dashTime;
    public float startDashTime;
    private int direction;


    float coolingdownCounter = 4;
    bool coolingdown = false;

    public float speed;
    public float movementx;
    public float jumpspeed;

    private int SceneIndex;

    //private bool jumpAllowed = false;

    private float periodLength = 10; //Seconds
    private float amount = 10; //Amount to add

    void Start()
    {
        dashTime = startDashTime;
        currentState = "Idle";
        SetCharacterState(currentState);
        SceneIndex = SceneManager.GetActiveScene().buildIndex;
        
    }

    private void Awake()
    {
        Infoscript curr = Infoscript.instance;
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
        //pFloat = GameObject.FindGameObjectsWithTag("ParticleFloat")[0];
        //tempObject = pFloat;
        //tempObject.SetActive(false);

    }

    void Update()
    {
        if (controllable == true)
        {

            //Debug.Log(isGrounded()); 
            //Debug.Log("= SceneIndex = " + SceneIndex + " , " + "isgrounded() = " + isGrounded());
            
            if (abilities == true && isGrounded() == false)
            {       
                Floating();
            }

            if (Input.GetKeyDown(KeyCode.E)) { isDashButtonDownE = true; }
            if (Input.GetKeyDown(KeyCode.W)) { isDashButtonDownW = true; }

            if (isDashButtonDownE && Player.currDash > 0)
            {
                if (this.transform.localScale.x > 0)
                {
                    rb.velocity = new Vector2(1f * speed * 60f, rb.velocity.y);
                    Player.currDash -= 1;
                    Infoscript.instance.UpdateDashAmulette();
                }
                else
                {
                    rb.velocity = new Vector2(-1f * speed * 60f, rb.velocity.y);
                    Player.currDash -= 1;
                    Infoscript.instance.UpdateDashAmulette();
                    
                }
                isDashButtonDownE = false;
            }

            else if (isDashButtonDownW && Player.currDash > 0)
            {
                Debug.Log("W down");
                //rb.AddForce(transform.forward * 100f);
                rb.velocity = new Vector2(movementx * speed, 1f * 15f);
                Player.currDash -= 1;
                Infoscript.instance.UpdateDashAmulette();

                
                isDashButtonDownW = false;
            }
            else
            {
                Move();
                /*if (regenDash != null)
                {
                    StopCoroutine(regenDash);
                }
                */

                regenDash = StartCoroutine(RegenDash());
                //Debug.Log(Player.instancePlayer.getCurrDash());
            }

        }
        
        if (Input.GetMouseButtonDown(1))
        {
            //Player.HealthPoints -= 1;
            Infoscript.instance.DamageHealthpoints(1);
        }

        /*
Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0f, 0f);
transform.position += movement * Time.deltaTime * moveSpeed;
Physics.gravity = new Vector3(0, 0.0F, 0);
*/
    }


    public IEnumerator RegenDash()
    {
        Debug.Log("nixpassiert");
        Debug.Log("Maxdash : " + Player.maxDash + " CurrDash : " + Player.currDash);

        yield return new WaitForSeconds(0.2f);
        Debug.Log("nixpassiert2");

        if (Player.currDash < Player.maxDash)
        {
            if (isGrounded()) { 
            Player.currDash += 1;
            Infoscript.instance.UpdateDashAmulette();
            yield return regenTickDash;
            }
        }
        regenDash = null;
    }

    public void Move()
    {
        movementx = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(movementx * speed , rb.velocity.y);

        if (movementx != 0)
        {
            if (!currentState.Equals("Jumping") && isGrounded() == true)
            {
                if (speed > 6)
                {
                    SetCharacterState("Running");
                    
                }
                else
                {
                    SetCharacterState("Walking");
                }
                
            }

            else
            {
                SetCharacterState("Floating");
            }

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
            if (!currentState.Equals("Jumping") && isGrounded())
            {
                SetCharacterState("Idle");
            }
            else
            {
                SetCharacterState("Floating");
            }
        }

        if (Input.GetButtonDown("Jump") && SceneIndex > 2)
        {
            if (isGrounded() == true)
            {
                Jump();
            }

        }
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpspeed);
        if (!currentState.Equals("Jumping"))
        {
            previousState = currentState;
        }
        SetCharacterState("Jumping");
    }

    public void SetAnimation(AnimationReferenceAsset animation, bool loop,float timeScale)
    {
        if (animation.name.Equals(currentAnimation))
        {
            return;
        }
        Spine.TrackEntry animationEntry = skeletonAnimation.state.SetAnimation(0, animation, loop);
        animationEntry.TimeScale = timeScale;
        animationEntry.Complete += AnimationEntry_Complete;
        //skeletonAnimation.state.SetAnimation(0, animation, loop).TimeScale = timeScale;
        currentAnimation = animation.name;
    }

    private void AnimationEntry_Complete(Spine.TrackEntry trackEntry)
    {
        if (currentState.Equals("Jumping") || currentState.Equals("Floating"))
        {
            SetCharacterState(previousState);
        }
    }

    public void SetCharacterState(string state)
    {
        
        if(state.Equals("Walking"))
        {
            SetAnimation(walking, true, 0.1f);
            //tempObject.SetActive(false);

            if (Soundcontrollerscript.soundInstance.currAudioSourceIndex != 4)
            {
                Soundcontrollerscript.soundInstance.StopAudioSource();
            }
            Soundcontrollerscript.soundInstance.playAudioSource(4);


        }
        else if (state.Equals("Running"))
        {
            SetAnimation(running, true, 0.35f);
            //tempObject.SetActive(false);

            if (Soundcontrollerscript.soundInstance.currAudioSourceIndex != 5)
            {
                Soundcontrollerscript.soundInstance.StopAudioSource();
            }

            Soundcontrollerscript.soundInstance.playAudioSource(5);
        }
        else if (state.Equals("Jumping"))
        {
            SetAnimation(jumping, false, 1f);
            //tempObject.SetActive(false);

            if (Soundcontrollerscript.soundInstance.currAudioSourceIndex != 6)
            {
                Soundcontrollerscript.soundInstance.StopAudioSource();
            }

            Soundcontrollerscript.soundInstance.playAudioSource(6);
        }
        else if (state.Equals("Floating"))
        {        
            SetAnimation(floating, false, 0.5f);

            //tempObject.SetActive(true);

            if (Soundcontrollerscript.soundInstance.currAudioSourceIndex != 7 && Soundcontrollerscript.soundInstance.currAudioSourceIndex != 6)
            {
                Soundcontrollerscript.soundInstance.StopAudioSource();
            }

            Soundcontrollerscript.soundInstance.playAudioSource(7);

        }
        else
        {

            SetAnimation(idle, true, 0.5f);
            Soundcontrollerscript.soundInstance.StopAudioSource();
            //tempObject.SetActive(false);
        }
        currentState = state;
    }


    void Dash() 
    {
    
    }

    public void getBoost()
    {
        rb.velocity = new Vector2(rb.velocity.x, 10f);
    }

    void Floating()
        {
        if (Input.GetButton("Fire1") && SceneIndex > 2)
        {
            if (!currentState.Equals("Floating"))
            {
                previousState = currentState;
            }
            SetCharacterState("Floating");
            if (FloatValue > 0) { 
            rb.gravityScale = 0.1f;
            }
            if (FloatValue < 1)
            {
                FloatValue = 0;
                rb.gravityScale = 0.8f;
            }
            }
        else {rb.gravityScale = 0.8f;}
        }

    private bool canMove(Vector3 dir, float distance) {
        return Physics2D.Raycast(transform.position, dir, distance).collider == null;
    }



    private bool isGrounded() {
        float extraHeightText = 0.03f;
        //RaycastHit2D raycastHit = Physics2D.Raycast(bc.bounds.center, new Vector2(0,-1), bc.bounds.extents.y + extraHeightText, platformLayerMask);
        RaycastHit2D raycastHitLeft = Physics2D.Raycast(new Vector2(bc.bounds.center.x - bc.bounds.extents.x, bc.bounds.center.y), new Vector2(0, -1), bc.bounds.extents.y + extraHeightText, platformLayerMask);
        RaycastHit2D raycastHitRight = Physics2D.Raycast(new Vector2(bc.bounds.center.x + bc.bounds.extents.x, bc.bounds.center.y), new Vector2(0, -1), bc.bounds.extents.y + extraHeightText, platformLayerMask);
        Color rayColor;
        if (raycastHitLeft.collider != null || raycastHitRight.collider != null)
        //if (raycastHit.collider != null && raycastHitLeft.collider != null && raycastHitRight.collider != null)
            {
            rayColor = Color.green;
        }
        else {
            rayColor = Color.yellow;
        }
        //Debug.DrawRay(bc.bounds.center, Vector2.down * (bc.bounds.extents.y + extraHeightText), rayColor);
        Debug.DrawRay(new Vector2(bc.bounds.center.x + bc.bounds.extents.x, bc.bounds.center.y), Vector2.down * (bc.bounds.extents.y + extraHeightText), rayColor);
        Debug.DrawRay(new Vector2(bc.bounds.center.x - bc.bounds.extents.x, bc.bounds.center.y), Vector2.down * (bc.bounds.extents.y + extraHeightText), rayColor);
        //return raycastHit.collider != null && raycastHitLeft.collider != null && raycastHitRight.collider != null;
        return raycastHitLeft.collider != null || raycastHitRight.collider != null;
    }
}
