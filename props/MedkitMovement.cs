using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private float moveDistance;
    private bool moveUp;
    private float upMax;
    private float downMax;

    [Header("Setting")]
    [SerializeField] private float healHealth;
    [SerializeField] private float speed;

    void Start()
    {
        moveDistance = 0.1f;
        moveUp = false;

        upMax = transform.position.y + moveDistance;
        downMax = transform.position.y - moveDistance;
    }


    void Update()
    {
        // MOVE UP AND DOWN TO ATTRACT ATTENTION    
        if (moveUp)
        {
            if (transform.position.y < upMax)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y + speed * Time.deltaTime, transform.position.z);
            }
            else
            {
                moveUp = false;
            }
        }
        else
        {
            if (transform.position.y > downMax)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y - speed * Time.deltaTime, transform.position.z);
            }
            else
            {
                moveUp = true;
            }
        }
    }

    // WHEN PLAYER TOUCH IT, IT HEALS PLAYER
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<Health>().Increase(healHealth);

            gameObject.SetActive(false);
        }
    }
}
