using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSceneController : MonoBehaviour
{
    public Button back, restart;
    public AudioClip clickSound;
    private AudioSource audioSource;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        back.onClick.AddListener(BackToMenu);
        restart.onClick.AddListener(RestartLevel);
    }

    
    void Update()
    {
        
    }

    void PlaySound()
    {
        audioSource.PlayOneShot(clickSound);
    }
    void BackToMenu()
    {
        PlaySound();
        SceneManager.LoadScene("MenuScene");
    }
    void RestartLevel()
    {
        PlaySound();
        SceneManager.LoadScene("GameScene");
    }
}
