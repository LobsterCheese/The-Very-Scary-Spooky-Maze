using UnityEngine;

public class orderInLayer : MonoBehaviour
{

    private SpriteRenderer sprender;

    [SerializeField]
    private bool moving;
    [SerializeField]
    private int flashlight;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sprender = GetComponent<SpriteRenderer>();
        sprender.sortingOrder = (int)transform.localPosition.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (moving)
        {
            sprender.sortingOrder = (int)transform.localPosition.y - flashlight;
        }
    }
}
