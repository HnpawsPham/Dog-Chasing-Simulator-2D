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

        // HARD MODE ADJUST
        if(PlayerPrefs.GetInt("gameMode") == 1){
            respawnTime += respawnTime / 2;
        }
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
