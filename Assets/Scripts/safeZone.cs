using UnityEngine;

public class safeZone : MonoBehaviour
{

    [SerializeField]
    private int checkpointNum;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Guy"))
        {
            checkpointManager.instance.currentCheckpoint = checkpointNum;
            checkpointManager.instance.madeIt = true;
            checkpointManager.instance.checkpointPosition = transform.position;
        }

        /*
        if(collision.gameObject.CompareTag("Spook") || collision.gameObject.CompareTag("Skelly"))
        {
            Destroy(collision.gameObject);
        }
        */
    }

}
