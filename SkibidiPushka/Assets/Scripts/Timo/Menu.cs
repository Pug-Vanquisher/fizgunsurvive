using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField, Tooltip("StartButton")] GameObject start;
    [SerializeField, Tooltip("EndGameText")] GameObject endGameText;
    private void Awake()
    {
        endGameText.SetActive(false);
    }
    public void Play()
    {
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }

    public void Retry()
    {
        SceneManager.LoadScene("Balance", LoadSceneMode.Single);
    }

    public void EndGame()
    {
        endGameText.SetActive(true);
        start.SetActive(false);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
