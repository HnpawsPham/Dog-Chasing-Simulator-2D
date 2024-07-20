using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverScreen;
    
    void Start()
    {
        
    }

    private void Awake() {
        gameOverScreen.SetActive(false);
    }

    void Update()
    {
        
    }

    public void GameOver(){
        StartCoroutine(EndGame());
    }

    // GAME OVER OPTIONS HANDLE
    public void Restart(){
        SceneManager.LoadScene(Mathf.Max(SceneManager.GetActiveScene().buildIndex, 2));
    }

    public void Menu(){
        SceneManager.LoadScene(0);
    }

    public void Quit(){
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
    }

    private IEnumerator EndGame(){
        yield return new WaitForSeconds(1.5f);

        SoundPlayer.instance.ClearInGameSounds();

        gameOverScreen.SetActive(true);
        SoundPlayer.instance.Play("game over");
    }
}

    
