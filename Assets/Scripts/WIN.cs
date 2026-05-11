using UnityEngine;
using UnityEngine.SceneManagement;

public class WIN : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Guy"))
        {
            SceneManager.LoadScene("winScene");
        }
    }

}
