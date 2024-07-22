
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionsHandle : MonoBehaviour
{
    [SerializeField] private RectTransform playText;

    [Header("Setting: ")]
    [SerializeField] private float speed;
    [SerializeField] private float upMax;
    [SerializeField] private float downMax;
    private bool moveUp;

    private void Awake()
    {
        moveUp = true;
    }

    void Update()
    {
        // MAKE PLAY TEXT MOVE UP AND DOWN TO ATTRACT ATTENTION    
        if (moveUp)
        {
            if (playText.position.y < upMax)
            {
                playText.position = new Vector3(playText.position.x, playText.position.y + speed * Time.deltaTime, playText.position.z);
            }
            else
            {
                moveUp = false;
            }
        }
        else
        {
            if (playText.position.y > downMax)
            {
                playText.position = new Vector3(playText.position.x, playText.position.y - speed * Time.deltaTime, playText.position.z);
            }
            else
            {
                moveUp = true;
            }
        }
    }

    // OPTIONS HANDLE
    public void Quit()
    {
        Application.Quit();
    }

    public void OpenSetting()
    {
        SceneManager.LoadScene(1);
    }

    public void Play(){
        SceneManager.LoadScene(2);
    }
}

