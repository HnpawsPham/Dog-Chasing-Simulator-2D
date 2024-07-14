using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using TreeEditor;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;

public class playerAttack : MonoBehaviour
{
    private Animator anim;
    private playerMovement playerMovement;
    private playerState playerState;

    [SerializeField] private float attackCooldown;
    [SerializeField] private float shootCooldown;
    [SerializeField] private float rechargeCooldown;
    [SerializeField] private Transform ammoPoint;
    [SerializeField] private GameObject[] ammos;
    

    private float cooldownTimer = Mathf.Infinity;

    void Awake()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<playerMovement>();
        playerState = GetComponent<playerState>();
    }
    void Start()
    {
        attackCooldown = 0.3f;
        shootCooldown = 0.4f;
        rechargeCooldown = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        // ATTACK
        if(Input.GetMouseButton(0) && cooldownTimer > attackCooldown && playerState.canAttack()){
            Attack();
        }

        // SHOOT
        if(Input.GetMouseButton(1) && cooldownTimer > shootCooldown && playerState.canShoot() && playerState.ammoLeft > 0){
            Shoot();
        }

        // RECHARGE
        if(Input.GetKey(KeyCode.R) || playerState.ammoLeft == 0){
            if(cooldownTimer > rechargeCooldown && playerState.canRecharge()){
                Recharge();
            }
        }

        cooldownTimer += Time.deltaTime;
    }
    // ATTACK: PUT OUT GUN AND STRIKE
    private void Attack()
    {
        cooldownTimer = 0;
        anim.SetTrigger("attack");
    }
    // SHOOT BY GUN
    private void Shoot()
    {
        cooldownTimer = 0;
        anim.SetTrigger("shoot");

        playerState.ammoLeft--;

        ammos[findAmmo()].transform.position = ammoPoint.position;
        ammos[findAmmo()].GetComponent<Ammo>().SetDirection(Mathf.Sign(transform.localScale.x));
    }

    // RECHARGE GUN
    private void Recharge()
    {
        cooldownTimer = 0;
        anim.SetBool("recharge", true);

        for(int i=0; i < ammos.Length; i++){
            ammos[i].SetActive(false);
        }
    }

    // FIND DEATIVATED AMMO TO REUSE IT
    private int findAmmo(){
        for(int i=0; i < ammos.Length; i++){
            if(!ammos[i].activeInHierarchy){
                return i;
            }
        }
        return 0;
    }
}
