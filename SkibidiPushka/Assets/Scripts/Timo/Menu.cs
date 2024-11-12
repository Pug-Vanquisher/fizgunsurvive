using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField, Tooltip("StartButton")] GameObject start;
    [SerializeField, Tooltip("EndGameText")] GameObject endGameText;
    public AudioClip Startclip;
    public AudioClip brake;
    public AudioSource Musik;
    private bool endGame = false;
    private void Awake()
    {
        endGameText.SetActive(false);
        Musik.clip = Startclip;
        Musik.Play();
    }
    public void Play()
    {
        Time.timeScale = 1f;
        gameObject.SetActive(false);
        Musik.clip = brake;
        Musik.Play();
    }

    public void Retry()
    {
        SceneManager.LoadScene("Balance", LoadSceneMode.Single);
    }

    public void EndGame()
    {
        if (!endGame)
        {
            endGame = true;
            endGameText.SetActive(true);
            start.SetActive(false);
            Musik.clip = Startclip;
            Musik.Play();
        }
    }

    public void Exit()
    {
        Application.Quit();
    }
}
