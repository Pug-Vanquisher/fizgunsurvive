using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class ConsoleScript : MonoBehaviour
{
    public bool StartingEnd = false;
    private bool EndingStart = false;

    public TMP_Text Text;
    public Image Image;
    
    public BaseEffect[] effects;
    public RiftEffect riftEffect;
    public TextAsset errorFile;

    private string[] errors;

    void Start()
    {
        errors = errorFile.text.Split("\n");
        EventManager.Instance.Subscribe("Upscaling", UpscalingEvent);
    }
    void UpscalingEvent()
    {
        foreach (BaseEffect effect in effects)
        {
            EventManager.Instance.Subscribe(effect.Event, ModChoosen);

        }
        StartCoroutine(ConsoleStart());
    }
    public void ShowRiftEffect()
    {
        StartCoroutine(ConsoleRift());
    }



    private void Update()
    {
        if (!StartingEnd)
        {
            Image.color = Color.Lerp(Image.color, new Color(0, 0, 0, 0.6f), 0.1f);
        }
        if (EndingStart)
        {
            Image.color = Color.Lerp(Image.color, new Color(0, 0, 0, 0), 0.1f);
        }
    }
    void ModChoosen()
    {
        StartCoroutine(ConsoleEnd());
    }

    IEnumerator ConsoleStart()
    {
        StartCoroutine(pause());
        float lostTime = 3f;
        for(int i = 0; i< Random.Range(10, 16); i++)
        {
            Text.text = errors[Random.Range(0, errors.Length)] + "\n" + Text.text;
            float timeToWait = Random.Range(0.05f, 0.2f);
            lostTime -= timeToWait;
            yield return new WaitForSecondsRealtime(timeToWait);
        }
        for(int i = 0;  i < 6; i++)
        {
            Text.text = errors[Random.Range(0, errors.Length)] + "\n" + Text.text;
            lostTime -= 0.01f;
        }
        yield return new WaitForSecondsRealtime(lostTime);
        StartingEnd = true;
        Text.text = "[CRITICAL ERROR] Возврат невозможен. Выберите:\n\n";
        for(int i= 0; i < effects.Length; i++) 
        {
            Text.text += "[" + (i + 1).ToString() + "] - <color=#" + Hexcode(effects[i].Color)  + ">" + effects[i].EventName + "</color>\n" + effects[i].Description + "\n" + "+ неизвестный модификатор\n\n";
        }
    }

    IEnumerator pause()
    {
        while (Time.timeScale > 0.1f)
        {
            Time.timeScale = Mathf.Lerp(Time.timeScale, 0, 0.01f);
            yield return new WaitForSecondsRealtime(0.01f);

        }
        Time.timeScale = 0.01f;
    }

    IEnumerator unpause()
    {
        while (Time.timeScale > 0.9f)
        {
            Time.timeScale = Mathf.Lerp(Time.timeScale, 1, 0.01f);
            yield return new WaitForSecondsRealtime(0.01f);
        }
        Time.timeScale = 1f;
    }

    IEnumerator ConsoleEnd()
    {
        Text.text = "Выбор сделан. Распаковка протоколов.";
        yield return new WaitForSecondsRealtime(0.2f);
        int percent = 0;
        while(percent < 100)
        {
            Text.text = "Распаковка " + percent + "%." + "\n" + Text.text;
            percent = Mathf.Clamp(percent + Random.Range(15, 26), 0, 101);
            yield return new WaitForSecondsRealtime(Random.Range(0.01f, 0.05f));
        }
        Text.text = "Ошибка при распаковке." + "\n" + Text.text; ;
        EndingStart = true;
        StartCoroutine(unpause());
        yield return new WaitForSecondsRealtime(0.5f);
        Text.text = "";
    }

    IEnumerator ConsoleRift()
    {
        StartCoroutine(pause());
        Text.text = "Получено сторонне решение. Распаковка протоколов.";
        yield return new WaitForSecondsRealtime(0.2f);
        Text.text = "Получено сообщение: '" + riftEffect.Event +"'\n" + Text.text;
        StartCoroutine(unpause());
        yield return new WaitForSecondsRealtime(2f);
        Text.text = "";
    }

    string Hexcode(Color color)
    {
        string red = (Mathf.RoundToInt(color.r * 222f)).ToString("X2");
        string green = (Mathf.RoundToInt(color.g * 222f)).ToString("X2");
        string blue = (Mathf.RoundToInt(color.b * 222f)).ToString("X2");
        return red + green + blue;

    }
}
