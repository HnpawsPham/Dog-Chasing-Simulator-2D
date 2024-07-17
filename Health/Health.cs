using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Setting: ")]
    [SerializeField]private float total;
    [SerializeField] private GameObject healEffect;

    public float current { get; private set; }
    public bool isDead;
    public bool isHurt;

    private Animator anim;

    void Start()
    {
        isDead = false;
        isHurt = false;
        current = total;

        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(healEffect.activeInHierarchy){
            Invoke("endHealEffect", 1f);
        }
    }

    public IEnumerator Hurt(){
        isHurt = true;

        yield return new WaitForSeconds(1);

        isHurt = false;
    }

    public void Decrease(float damage)
    {
        current = Mathf.Clamp(current - damage, 0, total);

        if (current > 0)
        {
            anim.SetTrigger("hurt");
            
            StartCoroutine(Hurt());
        }
        else
        {
            if (!isDead)
            {
                anim.SetTrigger("die");

                // PLAYER
                if(GetComponent<playerMovement>() != null){
                    GetComponent<playerMovement>().enabled = false;
                }

                // ENEMY
                if(GetComponentInParent<EnemyMovement>() != null){
                    GetComponentInParent<EnemyMovement>().enabled = false;
                }

                if(GetComponent<EnemyAttack>() != null){
                    GetComponent<EnemyAttack>().enabled = false;
                }
                

                isDead = true;
            }
        }
    }
    
    public void Increase(float health){
        current = Mathf.Clamp(current + health, current, total);

        healEffect.SetActive(true);
    }
    private void endHealEffect(){
        healEffect.SetActive(false);
    }

    private void HideAfterDeath(){
        gameObject.SetActive(false);
    }
}