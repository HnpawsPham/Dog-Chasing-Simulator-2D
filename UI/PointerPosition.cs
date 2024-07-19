using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointerPosition : MonoBehaviour
{
    [SerializeField] private RectTransform[] options;

    [Header("Sounds: ")]
    [SerializeField] private AudioClip selectSound;
    [SerializeField] private AudioClip changeSound;

    [Header("Pointer: ")]
    [SerializeField] private int type;
    [SerializeField] private int speed;
    [SerializeField] private float maxIn;
    [SerializeField] private float maxOut;

    private bool moveIn;
    private RectTransform rect;
    [SerializeField] private RectTransform otherPointer;

    private int currentPos;

    private void Awake() {
        moveIn = true;
        currentPos = 0;
        rect = GetComponent<RectTransform>();
    }

    void Update()
    {
        // CHANGE POS OF THE POINTER
        if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) || (currentPos > 0 && Input.GetKeyDown(KeyCode.LeftArrow))){
            ChangePos(-1);
        }
        else if(Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow) || (currentPos > 0 && Input.GetKeyDown(KeyCode.RightArrow))){
            ChangePos(1);
        }

        // SELECT OPTIONS
        if(Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return)){
            Select();
        }

        // MAKE KAWAII ANIMATION
        if(type == 2){
            if(moveIn){
                if(rect.position.x < maxIn){
                    rect.position = new Vector3(rect.position.x + speed * Time.deltaTime, rect.position.y, rect.position.z);
                    otherPointer.position = new Vector3(otherPointer.position.x - speed * Time.deltaTime, otherPointer.position.y, otherPointer.position.z);
                }
                else{
                    moveIn = false;
                }
            }
            else{
                if(rect.position.x > maxOut){
                    rect.position = new Vector3(rect.position.x - speed * Time.deltaTime, rect.position.y, rect.position.z);
                    otherPointer.position = new Vector3(otherPointer.position.x + speed * Time.deltaTime, otherPointer.position.y, otherPointer.position.z);
                }
                else{
                    moveIn = true;
                }
            }
        }
    }

    private void ChangePos(int newPos){
        currentPos += newPos;

        if(newPos != 0){
            ShortSounds.instance.Play(changeSound);
        }

        if(currentPos < 0){
            currentPos = options.Length - 1;
        }
        else if(currentPos > options.Length - 1){
            currentPos = 0;
        }

        // TYPES MENU
        switch(type){
            // ROTATE & LEFT
            case 1:
                rect.position = new Vector3(options[currentPos].position.x + options[currentPos].sizeDelta.x / 2, 
                options[currentPos].position.y - 50, 0);
                break;

            // KAWAII 
            case 2:
                rect.position = new Vector3(options[currentPos].position.x - Mathf.Abs(options[currentPos].sizeDelta.x * 5), 
                options[currentPos].position.y, 0);

                otherPointer.position = new Vector3(options[currentPos].position.x + Mathf.Abs(options[currentPos].sizeDelta.x * 5), 
                options[currentPos].position.y, 0);

                break;

            default:
                break;
        }
    }

    private void Select(){
        ShortSounds.instance.Play(selectSound);

        // ACCESS BUTTON COMPONENT
        options[currentPos].GetComponent<Button>().onClick.Invoke();
    }
}
