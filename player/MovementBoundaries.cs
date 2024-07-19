using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementBoundaries : MonoBehaviour
{
    [SerializeField] private LayerMask boundaryLayer;
    [SerializeField] private LayerMask checkpointLayer;
    [SerializeField] private Transform nearestCheckpoint;

    private BoxCollider2D boxCollider;
    private Animator anim;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
    }


    void Update()
    {
        // BACK TO THE NEAREST CHECKPOINT
        if (TouchBoundary())
        {
            transform.position = nearestCheckpoint.position;
            anim.SetTrigger("respawn");
        }

        // SET NEW NEAREST CHECKPOINT
        SetNewCheckpoint();
    }

    private bool TouchBoundary()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.left, 0, boundaryLayer);

        return raycastHit.collider != null;
    }

    private void SetNewCheckpoint(){
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.left, 0, checkpointLayer);

        if(raycastHit.collider != null){
            nearestCheckpoint = raycastHit.collider.transform;
        }
    }
}
