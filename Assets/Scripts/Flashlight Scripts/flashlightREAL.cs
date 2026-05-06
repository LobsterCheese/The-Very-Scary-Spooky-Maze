using UnityEngine;
using UnityEngine.Rendering.Universal;


public class flashlightNew : MonoBehaviour
{

    Vector2 mousePos;

    private float alpha;

    [SerializeField]
    private GameObject beam;

    public GameObject player;

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(transform.up);

        transform.position = player.transform.position;

        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        transform.up = (mousePos - (Vector2)transform.position).normalized;

        //this turns the flashlight left
        if (transform.up.x < 0 && (transform.up.y < -1 || transform.up.y < 1))
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        //this turns the flashlight right
        else if(transform.up.x >= 0 && (transform.up.y < -1 || transform.up.y < 1))
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }
}
