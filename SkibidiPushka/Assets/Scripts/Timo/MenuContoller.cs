using System.Collections;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField] Menu menu;
    private bool _isPaused = true;

    private void Start()
    {
        StartCoroutine(Trash());
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_isPaused)
            {
                menu.gameObject.SetActive(false);
                _isPaused = false;
                Time.timeScale = 1;
            }
            else
            {
                menu.gameObject.SetActive(true);
                Time.timeScale = 0f;
                _isPaused = true;
            }
        }
    }
    IEnumerator Trash()
    {
        yield return new WaitForSeconds(0.1f);
        Time.timeScale = 0f;

    }
}
