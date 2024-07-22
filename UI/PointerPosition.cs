using UnityEngine;
using UnityEngine.UI;

public class PointerPosition : MonoBehaviour
{
    [SerializeField] private RectTransform[] options;

    [Header("Pointer setting: ")]
    [SerializeField] private int type;
    [SerializeField] private float moveTime;
    [SerializeField] private RectTransform otherPointer;

    private bool moveIn;
    private bool adjustedSpeed = false;
    private float speed;
    private float maxOut;
    private int currentPos;

    private RectTransform rect;
    private BoxCollider2D boxCollider;

    private void Awake()
    {
        moveIn = true;

        currentPos = 0;
        rect = GetComponent<RectTransform>();

        maxOut = Screen.width / 2 - options[currentPos].sizeDelta.x;
    }

    void Update()
    {
        // CHANGE POS OF THE POINTER
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) || (currentPos > 0 && (Input.GetKeyDown(KeyCode.LeftArrow)) || Input.GetKeyDown(KeyCode.A)))
        {
            ChangePos(-1);
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow) || (currentPos > 0 && (Input.GetKeyDown(KeyCode.RightArrow)) || Input.GetKeyDown(KeyCode.D)))
        {
            ChangePos(1);
        }

        // SELECT OPTIONS
        if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
        {
            Select();
        }

        // MAKE KAWAII ANIMATION
        if (type == 2)
        {
            // KEEP MOVE TIME STABLE EVEN IF THE TEXT SIZE IS DIFFERENT (IN + OUT = 1 SEC FOR EXAMPLE)
            if(!adjustedSpeed){
                speed = (Screen.width / 2 - maxOut) / moveTime;
                adjustedSpeed = true;
            }

            // CHECK IF 2 POINTER TOUCHES EACH OTHER
            if(rect.position.x + rect.sizeDelta.x / 1.5 >= otherPointer.position.x){
                moveIn = false;
            }   

            // MOVE IN AND OUT HANDLE
            if (moveIn)
            {
                rect.position = new Vector3(rect.position.x + speed * Time.deltaTime, rect.position.y, rect.position.z);
                otherPointer.position = new Vector3(otherPointer.position.x - speed * Time.deltaTime, otherPointer.position.y, otherPointer.position.z);
            }
            else
            {
                if (rect.position.x > maxOut)
                {
                    rect.position = new Vector3(rect.position.x - speed * Time.deltaTime, rect.position.y, rect.position.z);
                    otherPointer.position = new Vector3(otherPointer.position.x + speed * Time.deltaTime, otherPointer.position.y, otherPointer.position.z);
                }
                else{
                    moveIn = true;
                }
            }
        }
    }

    private void ChangePos(int newPos)
    {
        currentPos += newPos;
        adjustedSpeed = false;

        if (newPos != 0)
        {
            SoundPlayer.instance.Play("key press");
        }

        // INFINITE LOOP
        if (currentPos < 0)
        {
            currentPos = options.Length - 1;
        }
        else if (currentPos > options.Length - 1)
        {
            currentPos = 0;
        }

        // TYPES MENU
        switch (type)
        {
            // ROTATE & LEFT
            case 1:
                rect.position = new Vector3(options[currentPos].position.x + options[currentPos].sizeDelta.x / 2,
                options[currentPos].position.y - 50, 0);
                break;

            // KAWAII 
            case 2:
                maxOut = Screen.width / 2 - options[currentPos].sizeDelta.x;
                
                rect.position = new Vector3(options[currentPos].position.x - Mathf.Abs(options[currentPos].sizeDelta.x),
                options[currentPos].position.y, 0);

                otherPointer.position = new Vector3(options[currentPos].position.x + Mathf.Abs(options[currentPos].sizeDelta.x),
                options[currentPos].position.y, 0);

                break;

            default:
                break;
        }
    }

    private void Select()
    {
        SoundPlayer.instance.Play("mouse click");

        // ACCESS BUTTON COMPONENT
        options[currentPos].GetComponent<Button>().onClick.Invoke();
    }
}
