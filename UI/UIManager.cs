using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject gameWinScreen;
    [SerializeField] private GameObject pauseMenu;

    private bool isEnded = false;
    
    void Start()
    {
        
    }

    private void Awake() {
    }

    void Update()
    {
        // PAUSE GAME
        if(Input.GetKeyDown(KeyCode.Escape)){

            if(!pauseMenu.activeInHierarchy && !isEnded){
                SoundPlayer.instance.Stop("dog run");

                Pause();
            }
            else{
                Resume();
            }
        }
    }

    public void GameOver(){
        StartCoroutine(EndGame());
        isEnded = true;
    }

    public void GameWin(){
        StartCoroutine(WinGame());
        isEnded = true;
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

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    private IEnumerator EndGame(){
        yield return new WaitForSeconds(1.5f);

        SoundPlayer.instance.ClearInGameSounds();

        gameOverScreen.SetActive(true);
        SoundPlayer.instance.Play("game over");
    }

    private IEnumerator WinGame(){
        yield return new WaitForSeconds(1.5f);

        SoundPlayer.instance.ClearInGameSounds();

        gameWinScreen.SetActive(true);
        SoundPlayer.instance.Play("you win");
    }

    // PAUSE MENU
    public void Pause(){
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
    }
    public void Resume(){
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }
}

    
