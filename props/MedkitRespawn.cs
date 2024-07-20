using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedkitRespawn : MonoBehaviour
{
    private float respawnWait;
    [SerializeField] private float respawnTime;
    [SerializeField] private GameObject medkit;

    void Awake()
    {
        respawnWait = 0;
    }


    void Update()
    {
        // RESPAWN MEDKIT AFTER BEING COLLECTED
        if(!medkit.activeInHierarchy){
            respawnWait += Time.deltaTime;

            if(respawnWait >= respawnTime){
                respawnWait = 0;

                medkit.SetActive(true);
            }
        }
    }
}
