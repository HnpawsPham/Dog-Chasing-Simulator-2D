using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private GameObject healEffect;
    [SerializeField] private float total;
    public float current { get; private set; }
    private float maxHealth;
    public bool isDead;

    private Animator anim;

    void Start()
    {
        isDead = false;
        maxHealth = 100;
        current = 100;

        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(healEffect.activeInHierarchy){
            Invoke("endHealEffect", 1f);
        }
    }

    public void Decrease(float damage)
    {
        current = Mathf.Clamp(current - damage, 0, maxHealth);

        if (current > 0)
        {
            anim.SetTrigger("hurt");
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
        current = Mathf.Clamp(current + health, current, maxHealth);

        healEffect.SetActive(true);
    }
    private void endHealEffect(){
        healEffect.SetActive(false);
    }

    private void HideAfterDeath(){
        gameObject.SetActive(false);
    }
}
