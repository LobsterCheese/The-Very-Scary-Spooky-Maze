using UnityEngine;
using UnityEngine.SceneManagement;

public class toRoom : MonoBehaviour
{

    public string roomName;

    public void goToRoom()
    {
        SceneManager.LoadScene(roomName);
    }

}
