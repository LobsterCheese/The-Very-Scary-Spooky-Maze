using UnityEngine;
using UnityEngine.SceneManagement;

public class restart : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerUpgrades.instance.currentMarkers = playerUpgrades.instance.markerAmount;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
