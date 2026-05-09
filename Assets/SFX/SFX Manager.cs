using UnityEngine;

public class SFXManager : MonoBehaviour
{

    public static SFXManager instance;

    public AudioClip[] walking;
    public AudioClip[] spookJumpscare;
    public AudioClip[] spookDie;
    public AudioClip[] skelly;
    public AudioClip[] skellyJumpscare;
    public AudioClip[] henryJumpscare;
    public AudioClip[] flashlight;
    public AudioClip[] upgrade;
    public AudioClip[] marker;

    AudioSource source;

    private void Awake()
    {
        source = GetComponent<AudioSource>();

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

    }

    //this randomizes pitch and plays random clips for variety, use this method for sfx
    public void PlaySoundRandom(AudioClip[] clips)
    {
        AudioClip clip = clips[Random.Range(0, clips.Length)];
        source.pitch = Random.Range(0.80f, 1.10f);
        source.PlayOneShot(clip);
    }
}
