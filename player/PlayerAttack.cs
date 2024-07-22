using Unity.Mathematics;
using UnityEngine;

public class playerAttack : MonoBehaviour
{
    private Animator anim;
    private playerMovement playerMovement;
    private playerState playerState;

    [Header("Cool down time: ")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float shootCooldown;
    [SerializeField] private float rechargeCooldown;

    [Header("Ammo: ")]
    [SerializeField] private Transform ammoPoint;
    [SerializeField] private GameObject[] ammos;
    

    private float attackWait = Mathf.Infinity;
    private float shootWait = Mathf.Infinity;

    void Awake()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<playerMovement>();
        playerState = GetComponent<playerState>();

        // HARD MODE ADJUST
        if(PlayerPrefs.GetInt("gameMode") == 1){
            shootCooldown += shootCooldown / 2;
        }
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // ATTACK
        if(Input.GetMouseButton(0) && attackWait > attackCooldown && playerState.canAttack()){
            Attack();
        }

        // SHOOT
        if(Input.GetMouseButton(1) && shootWait > shootCooldown && playerState.canShoot() && playerState.ammoLeft > 0){
            Shoot();
        }

        // RECHARGE
        if(Input.GetKey(KeyCode.R) || playerState.ammoLeft == 0){
            if(playerState.canRecharge()){
                Recharge();
            }
        }

        attackWait += Time.deltaTime;
        shootWait += Time.deltaTime;
    }
    // ATTACK: PUT OUT GUN AND STRIKE
    private void Attack()
    {
        attackWait = 0;
        anim.SetTrigger("attack");

        if(playerState.HitEnemy()){}
    }

    // SHOOT BY GUN
    private void Shoot()
    {
        SoundPlayer.instance.Play("gun");

        shootWait = 0;
        anim.SetTrigger("shoot");

        playerState.ammoLeft--;

        ammos[findAmmo()].transform.position = ammoPoint.position;
        ammos[findAmmo()].GetComponent<Ammo>().SetDirection(Mathf.Sign(transform.localScale.x));
    }

    // RECHARGE GUN
    private void Recharge()
    {
        SoundPlayer.instance.Play("recharge");

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
