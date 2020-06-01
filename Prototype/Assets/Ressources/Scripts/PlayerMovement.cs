using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.Audio;
using Spine.Unity.Examples;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private LayerMask platformLayerMask;

    public SkeletonAnimation skeletonAnimation;
    public AnimationReferenceAsset idle, walking, floating, running, jumping, jumpfloat;
    public string currentState;
    public string previousState;
    public string currentAnimation;

    public ParticleSystem playerDust;
    public ParticleSystem playerDustDirChange;
    public ParticleSystem playerDustFloat;

    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private BoxCollider2D bc;

    public static float MaxJumpValue = 100;
    public static float JumpValue;

    public static float MaxDashValue = 2;
    public static float DashValue;

    private bool isDashButtonDownE = false;
    private bool isDashButtonDownW = false;


    private float timeGrounded;
    private GameObject pFloat;
    private GameObject tempObject;

    private WaitForSeconds regenTickDash = new WaitForSeconds(0.02f);
    public Coroutine regenDash;

    public static float MaxFloatValue = 100;
    public static float FloatValue;

    public static bool controllable = true;
    public static bool abilities = true;

    private bool isBoosting = false;

    private float dashAmount = 1f;
    public float dashSpeed;
    private float dashTime;
    public float startDashTime;
    private int direction;

    private bool playerFloating = false;

    float coolingdownCounter = 4;
    bool coolingdown = false;

    public float speed;
    public float movementx;
    public float jumpspeed;

    private int SceneIndex;
    cameraShake cameraShake;

    void Start()
    {
        dashTime = startDashTime;
        currentState = "Idle";
        SetCharacterState(currentState);
        SceneIndex = SceneManager.GetActiveScene().buildIndex;
        cameraShake = GameObject.Find("CameraControllerContainer").GetComponent<cameraShake>();
        //Infoscript.instance.UpdateDashAmulette();

    }

    private void Awake()
    {
        Infoscript curr = Infoscript.instance;
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();

    }

    void Update()
    {

        if (controllable == true)
        {
            if (abilities == true && isGrounded() == false)
            {
                Floating();
            }

            if (Input.GetKeyDown(KeyCode.E)) { isDashButtonDownE = true; }
            if (Input.GetKeyDown(KeyCode.W)) { isDashButtonDownW = true; }

            /*
            if (isDashButtonDownE && Player.instancePlayer.currDash > 0 && !isGrounded())
            {
                cameraShake.Shake(0.4f, 0.1f);
                if (this.transform.localScale.x > 0)
                {
                    rb.velocity = new Vector2(1f * speed * 60f, rb.velocity.y);
                    //rb.transform.Translate(speed * Time.deltaTime,0,0);
                    Player.instancePlayer.currDash -= 1;
                    Infoscript.instance.UpdateDashAmulette();
                }
                else
                {
                    //rb.transform.Translate(speed * Time.deltaTime * -1, 0, 0);
                    rb.velocity = new Vector2(-1f * speed * 60f, rb.velocity.y);
                    Player.instancePlayer.currDash -= 1;
                    Infoscript.instance.UpdateDashAmulette();

                }
                isDashButtonDownE = false;
            }
            */
            if (isDashButtonDownW && Player.instancePlayer.currDash > 0)
            {
                cameraShake.Shake(0.4f, 0.1f);
                rb.gravityScale = 0.1f;
                rb.velocity = new Vector2(movementx * speed, 1f * 15f);
                Player.instancePlayer.currDash -= 1;
                Infoscript.instance.UpdateDashAmulette();

                timeGrounded = 0;
                isDashButtonDownW = false;
            }
            else
            {
                Move();
                isDashButtonDownE = false;
                isDashButtonDownW = false;
                if (isGrounded()) 
                { 
                    regenDash = StartCoroutine(RegenDash());
                }
            }

        }

        /*if (Input.GetMouseButtonDown(1))
        {
            Infoscript.instance.DamageHealthpoints(1);
        }
        */
    }


    private void FixedUpdate()
    {

    }
    void changeParticleColor(Color particleColor)
    {
        var main = playerDustFloat.main;
        main.startColor = particleColor;
    }

    void DustFloat()
    {
        playerDustFloat.Play();
    }
    public void createDust() 
    {
        playerDust.Play();
    }

    void playDirChange() 
    {
        playerDustDirChange.Play();
    }
    void StopDirChange()
    {
        playerDustDirChange.Stop();
    }

    void StopDustFloat()
    {
        playerDustFloat.Stop();
    }

    public IEnumerator RegenDash()
    {
        yield return new WaitForSeconds(0.2f);

        if (Player.instancePlayer.currDash < Player.instancePlayer.maxDash)
        {
            if (isGrounded() && timeGrounded > 1) { 
            Player.instancePlayer.currDash += 1;
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
                    playDirChange();

                }
                else
                {
                    SetCharacterState("Walking");
                    playDirChange();
                }
                
            }

            else
            {
                SetCharacterState("Floating");
            }

            if (movementx > 0)
            {
                transform.localScale = new Vector2(0.5f, 0.5f);
                if (isGrounded())
                { 
                }

            }
            else
            {
                transform.localScale = new Vector2(-0.5f, 0.5f);
                if (isGrounded())
                {
                }
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
                StopDirChange();
            }
        }

        if (Input.GetButtonDown("Jump") && SceneIndex > 3)
        {
            if (isGrounded() == true)
            {
                Jump();
                StopDirChange();
                DustFloat();
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
            //SetCharacterState(previousState);
        }
    }

    public void SetCharacterState(string state)
    {
        
        if(state.Equals("Walking"))
        {
            SetAnimation(walking, true, 0.08f);
            StopDustFloat();
            playerFloating = false;

            if (Soundcontrollerscript.soundInstance.currAudioSourceIndex != 4)
            {
                Soundcontrollerscript.soundInstance.StopAudioSource();
            }

            Soundcontrollerscript.soundInstance.playAudioSource(4);


        }
        else if (state.Equals("Running"))
        {
            SetAnimation(running, true, 0.35f);
            StopDustFloat();
            playerFloating = false;

            if (Soundcontrollerscript.soundInstance.currAudioSourceIndex != 9 && Soundcontrollerscript.soundInstance.currAudioSourceIndex != 8)
            {
                Soundcontrollerscript.soundInstance.StopAudioSource();
            }
            //Soundcontrollerscript.soundInstance.StopAudioSource();
            if (transform.localScale.x > 0)
            {
                if(Soundcontrollerscript.soundInstance.currAudioSourceIndex == 8) {
                    Soundcontrollerscript.soundInstance.StopAudioSource();
                }
                Soundcontrollerscript.soundInstance.playAudioSource(9);
                
            }
            if (transform.localScale.x < 0)
            {
                if (Soundcontrollerscript.soundInstance.currAudioSourceIndex == 9)
                {
                    Soundcontrollerscript.soundInstance.StopAudioSource();
                }
                Soundcontrollerscript.soundInstance.playAudioSource(8);
            }

            //Soundcontrollerscript.soundInstance.playAudioSource(5);
        }
        else if (state.Equals("Jumping"))
        {
            StopDustFloat();
            playerFloating = false;
            SetAnimation(jumping, false, 1f);

            if (Soundcontrollerscript.soundInstance.currAudioSourceIndex != 6)
            {
                Soundcontrollerscript.soundInstance.StopAudioSource();
            }

            Soundcontrollerscript.soundInstance.playAudioSource(6);
        }
        else if (state.Equals("Floating"))
        {        
            SetAnimation(floating, true, 0.5f);
            playerFloating = true;
            DustFloat();
            StopDirChange();

            if (Soundcontrollerscript.soundInstance.currAudioSourceIndex != 7 && Soundcontrollerscript.soundInstance.currAudioSourceIndex != 6)
            {
                Soundcontrollerscript.soundInstance.StopAudioSource();
            }

            Soundcontrollerscript.soundInstance.playAudioSource(7);

        }
        else
        {

            SetAnimation(idle, true, 0.5f);

            if (playerFloating == true)
            {
                playDirChange();
                StopDustFloat();
                playerFloating = false;
            }
            
            Soundcontrollerscript.soundInstance.StopAudioSource();
        }
        currentState = state;
    }


    void Dash() 
    {
    
    }

    public void getBoost()
    {
        rb.velocity = new Vector2(rb.velocity.x, 15f);
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
                
                changeParticleColor(new Color(1, 0, 1, 1));
            }
            if (FloatValue < 1)
            {
                FloatValue = 0;
                rb.gravityScale = 0.8f;
                
            }
            }
        else 
        {
            rb.gravityScale = 0.8f;
            changeParticleColor(new Color(1, 1, 1, 1));
        }
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
            timeGrounded += Time.deltaTime;
            //Debug.Log(timeGrounded);
            }
            else
            {
                rayColor = Color.yellow;
            }
            //Debug.DrawRay(bc.bounds.center, Vector2.down * (bc.bounds.extents.y + extraHeightText), rayColor);
            Debug.DrawRay(new Vector2(bc.bounds.center.x + bc.bounds.extents.x, bc.bounds.center.y), Vector2.down * (bc.bounds.extents.y + extraHeightText), rayColor);
            Debug.DrawRay(new Vector2(bc.bounds.center.x - bc.bounds.extents.x, bc.bounds.center.y), Vector2.down * (bc.bounds.extents.y + extraHeightText), rayColor);
        //return raycastHit.collider != null && raycastHitLeft.collider != null && raycastHitRight.collider != null;
        //Debug.Log("timeGrounded" + timeGrounded);
        return raycastHitLeft.collider != null || raycastHitRight.collider != null;
        }

    public bool sideRaycast()
    {
        return true;
    }
}
