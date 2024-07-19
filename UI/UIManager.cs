using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverScreen;

    [Header("Sounds: ")]
    [SerializeField] private AudioClip gameOverSFX;
    
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
        gameOverScreen.SetActive(true);
        ShortSounds.instance.Play(gameOverSFX);
    }

    // GAME OVER OPTIONS HANDLE
    public void Restart(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Menu(){
        SceneManager.LoadScene(0);
    }

    public void Quit(){
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
    }
}
