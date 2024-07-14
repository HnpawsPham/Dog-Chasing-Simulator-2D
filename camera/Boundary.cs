using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boundary : MonoBehaviour
{
    [SerializeField] private Transform nextScene;
    [SerializeField] private Transform previousScene;
    [SerializeField] private cameraControl cam;


    private void OnTriggerEnter2D(Collider2D collison) {
        if(collison.tag == "Player"){
            if(collison.transform.position.x < transform.position.x){
                cam.Move(nextScene);
            }
            else{
                cam.Move(previousScene);
            }
        }
    }
}
