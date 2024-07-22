using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class StaticTrap : MonoBehaviour
{

    private Animator anim;
    private BoxCollider2D trap;
    [SerializeField]private float damage;
    private float resetTime;
    private float resetWait;

    void Start()
    {
        resetWait = 0;
        resetTime = 5;

        anim = GetComponent<Animator>();
        trap = GetComponent<BoxCollider2D>();

        // HARD MODE ADJUST
        if(PlayerPrefs.GetInt("gameMode") == 1){
            damage += damage / 2;
            resetWait -= resetWait / 2;
        }
    }


    void Update()
    {
        resetWait += Time.deltaTime;

        if(resetWait >= resetTime && !trap.enabled){
            resetWait = 0;
            trap.enabled = true;
            anim.SetTrigger("reset");
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            anim.SetTrigger("start");
            collision.GetComponent<Health>().Decrease(damage);

            trap.enabled = false;
        }
    }
}
