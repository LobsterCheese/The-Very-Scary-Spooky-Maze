using UnityEngine;

public class MonsterSFX : MonoBehaviour
{

    private AudioSource source;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    private void Update()
    {
        source.pitch = Random.Range(0.9f, 1.10f);
    }
}
