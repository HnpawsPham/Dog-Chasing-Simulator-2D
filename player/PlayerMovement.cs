using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;

public class playerMovement : MonoBehaviour
{
    private Rigidbody2D body;
    private Animator anim;
    private playerState playerState;
    private Health health;


    [SerializeField] private GameObject eye;
    [SerializeField] private GameObject ropeCover;
    [SerializeField] private float playerSpeed;
    [SerializeField] private float jumpHeight;
    [SerializeField] private Transform hangPoint;

    private float horizontalAxis;
    private float verticalAxis;

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        playerState = GetComponent<playerState>();
        health = GetComponent<Health>();
    }

    public void Start()
    {
        playerSpeed = 5f;
        jumpHeight = 10f;
    }

    void Update()
    {
        horizontalAxis = Input.GetAxis("Horizontal");
        verticalAxis = Input.GetAxis("Vertical");

        Walk();
        Jump();
        Run();
        Hide();
        Crounch();
        Sneak();
        Climb();

        Hang();
    }

    // MOVE LEFT AND RIGHT
    private void Walk()
    {
        if (playerState.onSurface() && !anim.GetBool("recharge"))
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

    }

    // RUN
    private void Run()
    {
        if (playerState.onSurface() && !anim.GetBool("recharge"))
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
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
        }
        else
        {
            anim.SetBool("isRunning", false);
            playerSpeed = 5;
        }
    }

    // JUMP
    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && playerState.onSurface())
        {
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
        if (playerState.isInsideCrateStack() && anim.GetBool("isCrounching"))
        {
            eye.SetActive(true);    // VISIBLE THE EYE ON TOP OF THE PLAYER
            body.GetComponent<Renderer>().material.color = new Color32(99, 99, 99, 237);   // DARKEN PLAYER
        }
        else
        {       // RESET
            eye.SetActive(false);
            body.GetComponent<Renderer>().material.color = Color.white;
        }
    }

    // HANG ON A ROPE
    private void Hang()
    {
        if (playerState.isTouchedHagingRope())
        {
            health.Decrease(100);

            anim.SetTrigger("hanged");

            body.transform.position = hangPoint.position;
            ropeCover.SetActive(true);  // VISIBLE ROPE COVER
            body.isKinematic = true; // TURN OFF PHYSICS SO CHARACTER WONT FALL OFF
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
