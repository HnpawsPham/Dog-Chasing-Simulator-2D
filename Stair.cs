using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stair : MonoBehaviour
{
    [SerializeField] private BoxCollider2D player;
    [SerializeField] private LayerMask stairEndPoint;
    [SerializeField] private LayerMask bridgeLayer;
    [SerializeField]private GameObject topBridge;


    void Start()
    {
    }


    void Update()
    {
        allowBridge();    
    }

    // IF PLAYER IS TOUCHING STAIR END POINT, ALLOW HIM TO STAND ON THE BRIDGE
    public void allowBridge(){
        RaycastHit2D raycastHit = Physics2D.BoxCast(player.bounds.center, player.bounds.size, 0, Vector2.right, 0.1f, stairEndPoint);
        RaycastHit2D raycast = Physics2D.BoxCast(player.bounds.center, player.bounds.size, 0, Vector2.right, 0.1f, bridgeLayer);
        
        if(raycastHit.collider != null){
            if(!topBridge.activeInHierarchy){
                topBridge.SetActive(true);
            }

            if(Input.GetKey(KeyCode.S) && topBridge.activeInHierarchy){
                topBridge.SetActive(false);
            }
        }

        if(raycast.collider == null){
             topBridge.SetActive(false);
        }
    }
}
