
using System;
using System.Collections;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    [Header("Moving boundary:")]
    [SerializeField] private GameObject leftSide;
    [SerializeField] private GameObject rightSide;

    [Header("Set up:")]
    [SerializeField] private Transform enemy;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private Rigidbody2D body;
    [SerializeField] public GameObject player;


    [Header("Enemy movement:")]
    [SerializeField] private float defaultSpeed;
    [SerializeField] private float maxSpeed;
    [SerializeField] public float jumpHeight;

    [Header("Layers: ")]
    [SerializeField] private LayerMask surfaceLayer;

    [Header("Wait time: ")]
    [SerializeField] private float idleTime;
    [SerializeField] private float jumpDelay;


    [SerializeField] private Animator anim;

    private float idleWait;
    private Vector3 currentScale;
    private bool movingLeft;
    private float speed;
    private EnemyAttack enemyAttack;

    void Awake()
    {
        currentScale = enemy.localScale;

        enemyAttack = GetComponentInChildren<EnemyAttack>();

        speed = defaultSpeed;
        idleWait = 0;
    }

    void Start()
    {
        movingLeft = true;
    }

    void Update()
    {

        if (!enemyAttack.PlayerInSight())
        {
            SoundPlayer.instance.Stop(gameObject.name + " run");
            // NORMAL SPEED WHEN NOT CHASING
            speed = defaultSpeed;

            anim.SetBool("isRunning", false);
            anim.SetBool("isWalking", true);

            // ENEMY MOVE AROUND
            leftSide.SetActive(true);
            rightSide.SetActive(true);

            if (movingLeft)
            {
                if (enemy.position.x >= leftSide.transform.position.x)
                {
                    MoveInDirection(-1);
                }
                else
                {
                    ChangeDirection();
                }
            }
            else
            {
                if (enemy.position.x <= rightSide.transform.position.x)
                {
                    MoveInDirection(1);
                }
                else
                {
                    ChangeDirection();
                }
            }
        }
        else
        {
            leftSide.SetActive(false);
            rightSide.SetActive(false);

            speed = maxSpeed;

            anim.SetBool("isWalking", false);
            anim.SetBool("isRunning", true);

            // CHANGE DIRECTION WHILE MOVING
            var direction = (player.transform.position.x >= enemy.position.x) ? 1 : -1;  
            enemy.localScale = new Vector3(Mathf.Abs(currentScale.x) * direction, currentScale.y, currentScale.z);

            // CHASE PLAYER 
            if(!GetComponentInChildren<Health>().isDead){
                SoundPlayer.instance.Play(gameObject.name + " run");
            }
            else{
                SoundPlayer.instance.Stop(gameObject.name + " run");
                this.enabled = false;
            }

            enemy.position = new Vector3(enemy.position.x + Time.deltaTime * direction * speed, enemy.position.y, enemy.position.z);

            if(Input.GetKeyDown(KeyCode.Space)){
                StartCoroutine(Jump());
            }
        }
    }


    private void ChangeDirection()
    {

        // AFTER REACHED TO THE OTHER SIDE, ENEMY TAKES A BREAK
        idleWait += Time.deltaTime;

        if (idleWait > idleTime)
        {
            movingLeft = !movingLeft;
        }
        else
        {
            anim.SetBool("isWalking", false);
            anim.SetBool("isRunning", false);
        }
    }

    // MOVE TO THE GIVEN DIRECTION
    private void MoveInDirection(int newDirection)
    {

        idleWait = 0;

        anim.SetBool("isWalking", true);

        // CHANGE DIRECTION WHILE MOVING
        enemy.localScale = new Vector3(Mathf.Abs(currentScale.x) * newDirection, currentScale.y, currentScale.z);

        enemy.position = new Vector3(enemy.position.x + Time.deltaTime * newDirection * speed, enemy.position.y, enemy.position.z);
    }

    // CHECK IF ENEMY IS ON THE GROUND
    public bool onSurface()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, surfaceLayer);
        return raycastHit.collider != null;
    }

    // MAKE ENEMY JUMP WHEN PLAYER JUMP
    IEnumerator Jump(){
        yield return new WaitForSeconds(jumpDelay);

        anim.SetTrigger("jump");
        body.velocity = new Vector2(body.velocity.x, jumpHeight);
    }
}