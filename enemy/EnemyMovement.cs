
using System.Data.Common;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    [SerializeField] private Transform leftSide;
    [SerializeField] private Transform rightSide;

    [SerializeField] private Transform enemy;
    [SerializeField] private float defaultSpeed;
    private float speed;

    [SerializeField] private float idleTime;
    private float idleWait;

    [SerializeField] private Animator anim;

    private Vector3 currentScale;
    private bool movingLeft;
    private EnemyAttack enemyAttack;

    void Awake()
    {
        currentScale = enemy.localScale;
        enemyAttack = GetComponent<EnemyAttack>();

        speed = defaultSpeed;
        idleWait = 0;
    }

    void Start()
    {
        movingLeft = true;
    }

    void Update()
    {
        // ENEMY MOVE AROUND
        if (movingLeft)
        {
            if (enemy.position.x >= leftSide.position.x)
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
            if (enemy.position.x <= rightSide.position.x)
            {
                MoveInDirection(1);
            }
            else
            {
                ChangeDirection();
            }
        }


        // IF ENEMY SEES PLAYER, IT WILL SPEED UP
        // if (enemyAttack.PlayerInSight())
        // {
        //     speed *= 2;
        // }
        // else
        // {
        //     speed = defaultSpeed;
        // }
    }

    private void ChangeDirection()
    {

        // AFTER REACHED TO THE OTHER SIDE, ENEMY TAKES A BREAK
        idleWait += Time.deltaTime;

        if (idleWait > idleTime)
        {
            movingLeft = !movingLeft;
        }

        anim.SetBool("isWalking", false);
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
