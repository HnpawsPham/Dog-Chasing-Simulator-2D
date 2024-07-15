using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [Header("Enemy sight: ")]
    [SerializeField] private float horizontalRange;
    [SerializeField] private float verticalRange;
    [SerializeField] private float sightDistance;
    [SerializeField] private float sightHeight;


    [Header("Enemy attack: ")]
    [SerializeField] private float attackCoolDown;
    [SerializeField] private float attackDamage;

    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private BoxCollider2D boxCollider;

    private float coolDownTime = Mathf.Infinity;

    private Animator anim;
    private Health playerHealth;
    private EnemyMovement enemyMovement;
    [SerializeField] private playerState playerState;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        enemyMovement = GetComponentInParent<EnemyMovement>();
        playerState.GetComponent<playerState>();
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
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.left, 1.0f, playerLayer);

        return raycastHit.collider != null && !playerHealth.isDead && !playerState.isInsideCrateStack();
    }

    // DRAW ENEMY'S ATTACK VIEW
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * horizontalRange * transform.localScale.x * sightDistance,
        new Vector3(boxCollider.bounds.size.x * horizontalRange, boxCollider.bounds.size.y * verticalRange, boxCollider.bounds.size.z));
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
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * horizontalRange * transform.localScale.x * sightDistance,
        new Vector3(boxCollider.bounds.size.x * horizontalRange, boxCollider.bounds.size.y * verticalRange, boxCollider.bounds.size.z),
        0, Vector2.left, 0.1f, playerLayer);

        if (raycastHit.collider != null)
        {
            playerHealth = raycastHit.transform.GetComponent<Health>();
        }

        return raycastHit.collider != null && !playerHealth.isDead && !playerState.isInsideCrateStack();
    }
}
