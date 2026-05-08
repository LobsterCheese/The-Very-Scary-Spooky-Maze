using NUnit.Framework.Interfaces;
using Unity.VisualScripting;
using UnityEngine;

public class deathScreenJuice : MonoBehaviour
{
    [SerializeField]
    private float offset;
    private float offsetGoal;

    private float startX;
    private float movementTimer = 0;

    [Header("Turn this on if you want rotation")]
    [SerializeField]
    private bool rotate;
    [SerializeField]
    private float rotAmt;
    private float startRotZ;

    private void Awake()
    {
        startRotZ = transform.rotation.z;
        startX = transform.position.x;
        offsetGoal = transform.position.x - offset;
        movementTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        float newPos = 0;

        movementTimer = Mathf.Min(1, movementTimer + Time.deltaTime * 0.5f);
        newPos = startX + (offsetGoal - startX) * -(Mathf.Cos(Mathf.PI * movementTimer) - 1) / 2;

        transform.position = new Vector3(newPos, transform.position.y, transform.position.z);

        float newRot = 0;

        newRot = startRotZ + (rotAmt - startRotZ) * -(Mathf.Cos(Mathf.PI * movementTimer) - 1) / 2;

        if (rotate)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, newRot);
        }
    }
}
