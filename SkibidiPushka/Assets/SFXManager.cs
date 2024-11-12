using UnityEngine;
using UnityEngine.Rendering;

public class SFXManager : MonoBehaviour
{
    public static SFXManager instance;
    [SerializeField] private AudioSource sfxObj;

    private void Awake()
    {
        if (instance == null) { instance = this; }

    }

    public void PlaySound(AudioClip clip, Transform pos)
    {
        AudioSource a = Instantiate(sfxObj, pos.position, Quaternion.identity);

        a.clip = clip;

        a.Play();

        float length = a.clip.length;

        Destroy(a.gameObject, length);
    }

    public void PlaySound(AudioClip[] clip, Transform pos)
    {
        int rand = Random.Range(0, clip.Length);

        AudioSource a = Instantiate(sfxObj, pos.position, Quaternion.identity);

        a.clip = clip[rand];

        a.Play();

        float length = a.clip.length;

        Destroy(a.gameObject, length);
    }
}
