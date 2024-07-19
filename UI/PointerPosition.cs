using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointerPosition : MonoBehaviour
{
    [SerializeField] private RectTransform[] options;
    [SerializeField] private AudioClip selectSound;
    [SerializeField] private AudioClip changeSound;
    private RectTransform rect;

    private int currentPos;

    private void Awake() {
        rect = GetComponent<RectTransform>();
    }

    void Start()
    {
        
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

        rect.position = new Vector3(options[currentPos].position.x + 80, 
        options[currentPos].position.y - 50, 0);
    }

    private void Select(){
        ShortSounds.instance.Play(selectSound);

        // ACCESS BUTTON COMPONENT
        options[currentPos].GetComponent<Button>().onClick.Invoke();
    }
}
