using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{


    public void Play()
    {
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }
    public void Retry()
    {
        SceneManager.LoadScene("StartLVL", LoadSceneMode.Single);
    }
    public void Exit()
    {
        Application.Quit();
    }
}
