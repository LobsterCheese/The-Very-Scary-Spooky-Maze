using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class deathScreenJuice : MonoBehaviour
{
    [SerializeField]
    private float offset;
    private float offsetGoal;

    //[Header("Make sure this value is negative if u want it to go up")]
    //[SerializeField]
    //private float offsetY;
    //private float offsetYGoal;

    //[Header("If true, x; If false, y")]
    //[SerializeField]
    //private bool xy;

    private float startX;
    private float startY;
    //[SerializeField]
    //private float movementTimer = 0;

    [Header("Turn this on if you want rotation")]
    [SerializeField]
    private bool rotate;
    [SerializeField]
    private float rotAmt;
    private float startRotZ;

    private float rotGoal;

    private float speed = 5f;

    private void Awake()
    {
        startY = transform.position.y;
        startRotZ = transform.rotation.z;
        startX = transform.position.x;

        rotGoal = startRotZ + rotAmt;
        offsetGoal = startX - offset;
        //movementTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //rotates the image
        if (rotate)
        {
            transform.rotation = Quaternion.Euler(startX, startY, rotGoal);
        }

        transform.localPosition = new Vector3(Mathf.Lerp(offset, startX, speed), startY, 0f);

        speed -= 0.5f * Time.deltaTime;

        if (speed < 0)
        {
            speed = 0f;
        }

    }
}
