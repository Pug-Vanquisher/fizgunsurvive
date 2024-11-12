using UnityEngine;
using UnityEngine.Rendering;

public class SFXManager : MonoBehaviour
{
    public static SFXManager instance;
    [SerializeField] private AudioSource sfxObj;
    private Transform player;
    private void Awake()
    {
        if (instance == null) { instance = this; }

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
        }
    }

    public void PlaySound(AudioClip clip, Transform pos)
    {
        if ((player.position - pos.position).magnitude < 15f)
        {
            AudioSource a = Instantiate(sfxObj, pos.position, Quaternion.identity);

            a.clip = clip;

            a.Play();

            float length = a.clip.length;

            Destroy(a.gameObject, length);
        }
    }

    public void PlaySound(AudioClip[] clip, Transform pos)
    {
        if ((player.position - pos.position).magnitude < 15f)
        {
            int rand = Random.Range(0, clip.Length);

            AudioSource a = Instantiate(sfxObj, pos.position, Quaternion.identity);

            a.clip = clip[rand];

            a.Play();

            float length = a.clip.length;

            Destroy(a.gameObject, length);
        }
    }
}
