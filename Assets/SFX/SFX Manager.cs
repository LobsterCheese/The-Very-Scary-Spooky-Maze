using UnityEngine;

public class SFXManager : MonoBehaviour
{

    public static SFXManager instance;

    public AudioClip[] walking;
    public AudioClip[] jumpscare;
    public AudioClip[] skelly;

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

    //plays jumpscare sound
    public void playJump(int monster)
    {
        /*
         * 0 is spook 
         * 1 is skelly
         * 2 is happy henry
         * 3 is jolly jerry
        */
        source.PlayOneShot(jumpscare[monster]);
    }
}
