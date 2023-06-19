using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MenuSceneController : MonoBehaviour
{
    public Button play, quit, info;
    public AudioClip clickSound;
    public Canvas infoCanvas;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        play.onClick.AddListener(StartGame);
        quit.onClick.AddListener(QuitGame);
        info.onClick.AddListener(InfoCanvas);
    }


    void PlaySound()
    {
        audioSource.PlayOneShot(clickSound);
    }

    void StartGame()
    {
        PlaySound();
        SceneManager.LoadScene("GameScene");
    }

    void QuitGame()
    {
        PlaySound();
        Debug.Log("GAME QUIT");
        Application.Quit();
    }

    void InfoCanvas()
    {
        PlaySound();
        infoCanvas.gameObject.SetActive(!infoCanvas.gameObject.activeSelf);
    }
}
