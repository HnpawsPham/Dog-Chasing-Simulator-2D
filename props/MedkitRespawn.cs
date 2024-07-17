using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedkitRespawn : MonoBehaviour
{
    private float respawnWait;
    [SerializeField] private float respawnTime;
    [SerializeField] private GameObject medkit;

    void Start()
    {
        respawnWait = Mathf.Infinity;
    }


    void Update()
    {
        // RESPAWN MEDKIT AFTER BEING COLLECTED
        respawnWait += Time.deltaTime;

        if(!medkit.activeInHierarchy && respawnWait >= respawnTime){
            respawnWait = 0;

            medkit.SetActive(true);
        }
    }
}
