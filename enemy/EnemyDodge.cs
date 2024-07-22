using UnityEngine;

public class EnemyDodge : MonoBehaviour
{
    [SerializeField] private float dodgeDistance;
    [SerializeField] private LayerMask ammoLayer;

    private BoxCollider2D boxCollider;
    private Rigidbody2D body;
    private Animator anim;

    private void Awake() {
        boxCollider = GetComponent<BoxCollider2D>();
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if(AmmoAhead()){
            anim.SetTrigger("jump");
            body.velocity = new Vector2(body.velocity.x, GetComponentInParent<EnemyMovement>().jumpHeight);
        }
    }

    private bool AmmoAhead(){
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center,
        boxCollider.bounds.size, 0, Vector2.left, dodgeDistance, ammoLayer);

        return raycastHit.collider != null;
    }
}
