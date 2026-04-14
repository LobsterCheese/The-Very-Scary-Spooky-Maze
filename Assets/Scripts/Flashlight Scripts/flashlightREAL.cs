using UnityEngine;

public class flashlightNew : MonoBehaviour
{

    Vector2 mousePos;

    public GameObject player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position;

        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        transform.up = (mousePos - (Vector2)transform.position).normalized;
    }
}
