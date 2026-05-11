using UnityEngine;

public class freezeRemover : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Guy")
        {
            Destroy(gameObject);
        }
    }

}
