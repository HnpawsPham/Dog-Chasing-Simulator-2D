using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private float range;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private BoxCollider2D boxCollider;

    [SerializeField] private float attackCoolDown;
    [SerializeField] private float attackDamage;
    [SerializeField] private float sightDistance;

    private float coolDownTime = Mathf.Infinity;

    private Animator anim;
    private Health playerHealth;
    private EnemyMovement enemyMovement;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        enemyMovement = GetComponentInParent<EnemyMovement>();
    }

    void Start()
    {

    }


    void Update()
    {
        coolDownTime += Time.deltaTime;

        if (CanAttack())
        {
            anim.SetBool("isWalking", false);
            anim.SetBool("isRunning", true);

            if (coolDownTime >= attackCoolDown)
            {
                coolDownTime = 0;

                anim.SetTrigger("bite");
            }
        }
        else
        {
            anim.SetBool("isRunning", false);
        }

        // IF PLAYER IS IN FRONT OF ITS SIGHT, IT WONT HAVE A BREAK
        if (enemyMovement != null)
        {
            enemyMovement.enabled = !CanAttack();
        }

    }

    public bool CanAttack()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * sightDistance,
        new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
        0, Vector2.left, 0, playerLayer);

        if (raycastHit.collider != null)
        {
            playerHealth = raycastHit.transform.GetComponent<Health>();
        }

        return raycastHit.collider != null;
    }

    // DRAW ENEMY'S ATTACK VIEW
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * sightDistance,
        new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }


    private void DamagePlayer()
    {
        if (CanAttack())
        {
            playerHealth.Decrease(attackDamage);
        }
    }

    // CHECK IF PLAYER IS IN FRONT OF ENEMY'S SIGHT
    public bool PlayerInSight()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.left, 0.1f, playerLayer);
        return raycastHit.collider != null;
    }
}
