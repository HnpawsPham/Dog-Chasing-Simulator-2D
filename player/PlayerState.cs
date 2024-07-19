using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class playerState : MonoBehaviour
{
    private float horizontalAxis;
    private float verticalAxis;
    private bool isHanged;
    private bool fall;

    [Header("Components: ")]
    private BoxCollider2D boxCollider;
    private Animator anim;

    [Header("Ammo: ")]
    [SerializeField] public int ammoLeft;


    [Header("Layer masks: ")]
    [SerializeField] private LayerMask surfaceLayer;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask crateStackLayer;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private LayerMask hangingRopeLayer;
    [SerializeField] private LayerMask enemyLayer;

    void Awake()
    {
        isHanged = false;
        fall = false;

        ammoLeft = 15;

        horizontalAxis = Input.GetAxis("Horizontal");
        verticalAxis = Input.GetAxis("Vertical");

        boxCollider = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();

    }
    void Start()
    {
    }

    void Update()
    {
        Fall();
    }

    // CHECK IF PLAYER IS ON A SURFACE
    public bool onSurface()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, surfaceLayer);
        return raycastHit.collider != null || isGrounded();
    }

    // CHECK IF PLAYER IS STANDING ON THE GROUND
    public bool isGrounded()
    {


        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);

        if (raycastHit.collider != null)
        {
            if (fall)
            {
                anim.SetTrigger("hurt");
            }
        }
        return raycastHit.collider != null;
    }

    // CHECK IF PLAYER IS ABLE TO ATTACK
    public bool canAttack()
    {
        return Input.GetMouseButtonDown(0) && onSurface();
    }

    // CHECK IF PLAYER IS ABLE TO SHOOT
    public bool canShoot()
    {
        return horizontalAxis == 0 && onSurface();
    }

    // CHECK IF PLAYER CAN RECHARGE
    public bool canRecharge()
    {
        return horizontalAxis == 0 && onSurface();
    }

    // CHECK IF PLAYER IS INSIDE CRATE STACK
    public bool isInsideCrateStack()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, crateStackLayer);
        return raycastHit.collider != null;
    }


    // CHECK IF PLAYER IS TOUCHING THE HANGING ROPE
    public bool isTouchedHagingRope()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.up, 0.1f, hangingRopeLayer);
        return !onSurface() && raycastHit.collider != null && !isHanged;
    }

    // DELETE HANGED ANIM
    private void DeativateHangAnim()
    {
        isHanged = true;
        anim.ResetTrigger("hanged");
    }

    // FALL
    private void Fall()
    {
        if (transform.position.y > 5f)
        {
            fall = true;
        }
    }

    // RESET FALL
    private void ResetFall()
    {
        fall = false;
        
        anim.ResetTrigger("hurt");
    }

    // CHECK IF PLAYER CAN CLIMB
    public bool canClimb()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.left, 0.1f, wallLayer);
        return raycastHit.collider != null;
    }

    //RESET RECHARGE ANIM
    private void ResetRecharge(){
        anim.SetBool("recharge", false);
    }

    // REFILL AMMOS AFTER RECHARGE
    private void RefillAmmo(){
        ammoLeft = 15;
    } 

    // CHECK IF PLAYER ATTACKS HIT ENEMY
    public bool HitEnemy(){
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.left, 0.1f, enemyLayer);

        if(raycastHit.collider != null){
            raycastHit.transform.GetComponent<Health>().Decrease(5);
        }

        return raycastHit.collider != null;
    }
}
