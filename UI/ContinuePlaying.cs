
using UnityEngine;
using UnityEngine.SceneManagement;

public class ContinuePlaying : MonoBehaviour
{
    private int playingScene;

    void Start()
    {
        playingScene = 2;
    }

    void Update()
    {
        
    }

    // LOAD CURRENT SCENE
    public void Continue(){
        SceneManager.LoadScene(playingScene);
    }
}
