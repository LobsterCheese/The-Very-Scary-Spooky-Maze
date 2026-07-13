using UnityEngine;

public class audioLimiter : MonoBehaviour
{

    private AudioSource source;
    private GameObject target;
    [SerializeField]
    private float minDist;
    [SerializeField]
    private float maxDist;

    [SerializeField]
    private GameObject death;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        source = GetComponent<AudioSource>();
        target = GameObject.Find("Guy");
    }

    // Update is called once per frame
    void Update()
    {
        //gets distance from monster to player
        float dist = Vector3.Distance(transform.position, target.transform.position);

        //if the death screen is active, sound will be 0
        if (death.activeInHierarchy)
        {
            source.volume = 0;
        }
        else if (dist < minDist)
        {
            source.volume = 0.5f;
        }
        else if (dist > maxDist)
        {
            source.volume = 0;
        }
        else
        {
            source.volume = 0.5f - ((dist - minDist) / (maxDist - minDist));
        }
    }
}
