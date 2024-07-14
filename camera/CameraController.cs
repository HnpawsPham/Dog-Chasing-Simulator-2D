using System;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using Unity.VisualScripting;
using UnityEngine;

public class cameraControl : MonoBehaviour
{
 
    [SerializeField] private float speed;
    [SerializeField] private Transform player;

    private float posX;
    private float aheadDistance;
    private float aheadDirection;
    private float maxLeft;

    void Start()
    {
        speed = 2f;
        aheadDistance = 2;
    }

    // FOLLOW PLAYER
    void Update()
    {
        transform.position = new Vector3(player.position.x + aheadDirection + speed * Time.deltaTime, transform.position.y, transform.position.z);
        aheadDirection = Mathf.Lerp(aheadDirection, (aheadDistance * player.transform.localScale.x), speed * Time.deltaTime); 
    }

    public void Move(Transform target){
            posX = target.position.x;
    }
}
