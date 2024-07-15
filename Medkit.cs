using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private float health;
    private float moveDistance;
    private float moveSpeed;
    private bool moveUp;
    private float upMax;
    private float downMax;

    [SerializeField] private GameObject self;

    [Header("Setting")]
    [SerializeField] private float respawnTime;
    private float respawnWait;

    void Start()
    {
        health = 25;
        moveDistance = 0.1f;
        moveSpeed = 0.5f;
        moveUp = false;
        respawnWait = respawnTime;

        upMax = transform.position.y + moveDistance;
        downMax = transform.position.y - moveDistance;
    }


    void Update()
    {
        // MOVE UP AND DOWN TO ATTRACT ATTENTION    
        if(moveUp){
            if(transform.position.y < upMax){
                transform.position = new Vector3(transform.position.x, transform.position.y + moveSpeed * Time.deltaTime, transform.position.z);
            }
            else{
                moveUp = false;
            }
        }
        else{
            if(transform.position.y > downMax){
                transform.position = new Vector3(transform.position.x, transform.position.y - moveSpeed * Time.deltaTime, transform.position.z);
            }
            else{
                moveUp = true;
            }
        }

        // RESPAWM AFTER A FEW SECONDS

        respawnWait += Time.deltaTime;

        if(respawnWait >= respawnTime){
            respawnWait = 0;

            self.SetActive(true);
        }
    }

    // WHEN PLAYER TOUCH IT, IT HEALS PLAYER
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player"){
            collision.GetComponent<Health>().Increase(health);

            self.SetActive(false);
        }
    }
}
