using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;

public class playerMovement : MonoBehaviour
{
    private Rigidbody2D body;
    private Animator anim;
    private playerState playerState;
    private Health health;
    private Stamina stamina;

    [Header("Attached: ")]
    [SerializeField] private GameObject eye;
    [SerializeField] private GameObject ropeCover;
    [SerializeField] private Transform hangPoint;

    [Header("Setting: ")]
    [SerializeField] private float playerSpeed;
    [SerializeField] private float jumpHeight;
    [SerializeField] private float runStaminaLoss;
    [SerializeField] private float staminaRecover;

    [Header("Cool down time: ")]
    [SerializeField] private float staminaCoolDown;
    [SerializeField] private float jumpCoolDown; // ALLOW MULTIPLE JUMPS

    private float coolDownTime = Mathf.Infinity;
    private float jumpWait = Mathf.Infinity;
    private float horizontalAxis;
    private float verticalAxis;
    private UIManager uIManager;

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        playerState = GetComponent<playerState>();

        health = GetComponent<Health>();
        stamina = GetComponent<Stamina>();

        uIManager = FindObjectOfType<UIManager>();

        // HARD MODE ADJUST
        if(PlayerPrefs.GetInt("gameMode") == 1){
            stamina.total /= 3;
        }
    }

    public void Start()
    {

    }

    void Update()
    {
        horizontalAxis = Input.GetAxis("Horizontal");
        verticalAxis = Input.GetAxis("Vertical");

        if (!health.isDead)
        {
            Walk();
            Jump();
            Run();
            Hide();
            Crounch();
            Sneak();
            Climb();

            // HOLD SPACE TO JUMP HIGHER
            // if(Input.GetKeyUp(KeyCode.Space) && body.velocity.y > 0){
            //     body.velocity = new Vector2(body.velocity.x, body.velocity.y / 2);
            // }

            coolDownTime += Time.deltaTime;
            jumpWait += Time.deltaTime;
        }

        // GAME OVER IF PLAYER IS DEAD
        else
        {
            uIManager.GameOver();
            this.enabled = false;
        }

        Hang();
    }

    // MOVE LEFT AND RIGHT
    private void Walk()
    {
        if (playerState.onSurface())
        {
            body.velocity = new Vector2(horizontalAxis * playerSpeed, body.velocity.y);

            if (horizontalAxis > 0.001f)
            {
                transform.localScale = new Vector3(3, 3, 3);
            }
            else if (horizontalAxis < -0.001f)
            {
                transform.localScale = new Vector3(-3, 3, 3);
            }

            body.transform.rotation = Quaternion.Euler(0, 0, 0);

            anim.SetBool("isWalking", true);
        }

        if (horizontalAxis == 0)
        {        // STOP MOVING
            anim.SetBool("isWalking", false);
        }

        // REFILL STAMINA
        if (!anim.GetBool("isRunning") && coolDownTime >= staminaCoolDown)
        {
            stamina.Increase(staminaRecover);

            coolDownTime = 0;
        }
    }

    // RUN
    private void Run()
    {
        if (playerState.onSurface() && !anim.GetBool("recharge") && !stamina.isExhausted)
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                if (!health.isDead)
                {
                    SoundPlayer.instance.Play(gameObject.name + " run");
                }
                else{
                    SoundPlayer.instance.Stop(gameObject.name + " run");
                }

                anim.SetBool("isRunning", true);
                anim.SetBool("isWalking", false);

                playerSpeed = 10;
            }

            body.velocity = new Vector2(horizontalAxis * playerSpeed, body.velocity.y);

            if (horizontalAxis > 0.001f)
            {
                transform.localScale = new Vector3(3, 3, 3);
            }
            else if (horizontalAxis < -0.001f)
            {
                transform.localScale = new Vector3(-3, 3, 3);
            }

            body.transform.rotation = Quaternion.Euler(0, 0, 0);

            if (coolDownTime >= staminaCoolDown)
            {
                coolDownTime = 0;

                stamina.Decrease(runStaminaLoss);
            }
        }
        else
        {
            SoundPlayer.instance.Stop(gameObject.name + " run");

            anim.SetBool("isRunning", false);
            playerSpeed = 5;
        }
    }

    // JUMP
    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && jumpWait >= jumpCoolDown)
        {
            jumpWait = 0;

            SoundPlayer.instance.Play("player jump");

            body.velocity = new Vector2(body.velocity.x, jumpHeight);
            anim.SetBool("jump", true);
        }
        else
        {
            anim.SetBool("jump", false);
        }
    }

    // CROUNCH
    private void Crounch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (playerState.onSurface() && !anim.GetBool("isWalking"))
            {
                anim.SetBool("isCrounching", true);
            }
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            anim.SetBool("isCrounching", false);
        }
    }

    // SNEAK
    private void Sneak()
    {
        if (playerState.onSurface() && anim.GetBool("isWalking") && anim.GetBool("isCrounching"))
        {
            playerSpeed = 1;
        }
    }

    // HIDE
    private void Hide()
    {
        if (playerState.isInsideCrateStack() && anim.GetBool("isCrounching") && !health.isHurt)
        {
            if (!health.isDead)
            {
                SoundPlayer.instance.Play(gameObject.name + " hide");
            }

            eye.SetActive(true);    // VISIBLE THE EYE ON TOP OF THE PLAYER
            body.GetComponent<Renderer>().material.color = new Color32(99, 99, 99, 237);   // DARKEN PLAYER
        }
        else
        {
            SoundPlayer.instance.Stop(gameObject.name + " hide");

            eye.SetActive(false);
            body.GetComponent<Renderer>().material.color = Color.white;
        }
    }

    // HANG ON A ROPE
    private void Hang()
    {
        if (playerState.isTouchedHagingRope())
        {
            health.Decrease(health.total);

            anim.ResetTrigger("die");
            anim.SetTrigger("hanged");

            body.position = hangPoint.position;
            ropeCover.SetActive(true);

            // TURN OFF PHYSICS SO CHARACTER WONT FALL OFF
            body.constraints = RigidbodyConstraints2D.FreezePosition;
            body.isKinematic = true;
            body.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    // CLIMB
    private void Climb()
    {
        if (playerState.canClimb() && !playerState.onSurface())
        {
            anim.SetBool("isClimbing", true);
            body.gravityScale = 0;

            float climbSpeed = playerSpeed - 1;
            body.velocity = new Vector2(transform.position.x, verticalAxis * climbSpeed);
        }
        else
        {
            anim.SetBool("isClimbing", false);
            body.gravityScale = 3;
        }
    }
}
