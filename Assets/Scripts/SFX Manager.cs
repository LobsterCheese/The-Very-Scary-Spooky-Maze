using UnityEngine;

public class SFXManager : MonoBehaviour
{

    public static SFXManager instance;

    public AudioClip[] walking;
    public AudioClip[] jumpscare;

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

    public void PlaySoundRandom(AudioClip[] clips)
    {
        AudioClip clip = clips[Random.Range(0, clips.Length)];
        source.pitch = Random.Range(0.95f, 1.05f);
        source.PlayOneShot(clip);
        //Debug.Log("yo");
    }

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
