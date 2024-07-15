
using System;
using System.Data.Common;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    [Header("Moving boundary:")]
    [SerializeField] private GameObject leftSide;
    [SerializeField] private GameObject rightSide;

    [SerializeField] private Transform enemy;
    [SerializeField] private GameObject player;


    [Header("Enemy speed:")]
    [SerializeField] private float defaultSpeed;
    [SerializeField] private float maxSpeed;
    private float speed;
    private float currentDistance;

    [SerializeField] private float idleTime;
    private float idleWait;

    [SerializeField] private Animator anim;

    private Vector3 currentScale;
    private bool movingLeft;
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

            // CHASE PLAYER
            if(player.transform.position.x > enemy.position.x){
                enemy.position = new Vector3(enemy.position.x + Time.deltaTime * speed, enemy.position.y, enemy.position.z);
            }
            else{
                enemy.position = new Vector3(enemy.position.x + Time.deltaTime * -1 * speed, enemy.position.y, enemy.position.z);
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
}
