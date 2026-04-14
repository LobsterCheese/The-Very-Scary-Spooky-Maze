using UnityEngine;

public class skeletonBehavior : MonoBehaviour
{

    public Sprite[] skellyStills;

    private Rigidbody2D rb;

    private bool freeze;

    private SpriteRenderer sprender;
    private int randPose;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sprender = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!freeze)
        {
            randPose = Random.Range(0, skellyStills.Length);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        sprender.sprite = skellyStills[randPose];
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Flashlight") || collision.gameObject.CompareTag("animFreeze"))
        {
            freeze = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Flashlight") || collision.gameObject.CompareTag("animFreeze"))
        {
            freeze = false;
        }
    }

}
