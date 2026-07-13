using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class toRoom : MonoBehaviour
{

    [SerializeField]
    private int checkpointToLoadTo;
    /*
     * -1 is for the start of the maze
     * 0 is for the first checkpoint, so on and so forth
     */

    [SerializeField]
    private GameObject audioHolder;

    [SerializeField]
    private string roomName;

    [SerializeField]
    private GameObject fade;

    [SerializeField]
    private AnimationClip fadeTime;


    public void goToRoom()
    {
        SFXManager.instance.PlaySoundRandom(SFXManager.instance.bellClick);

        //loads player to maze but doesnt destroy upgrades
        checkpointManager.instance.startingFromScreen = true;
        checkpointManager.instance.currentCheckpoint = checkpointToLoadTo;

        //in case player couldnt move, they can now
        Time.timeScale = 1f;
        playerUpgrades.instance.dontMove = 1f;

        //this is a really stupid way to do it manually but its easy so DO IT
        if (checkpointToLoadTo == -1)
        {
            checkpointManager.instance.checkpointPosition = new Vector3(-30, 28, 0);
        }
        else if(checkpointToLoadTo == 0)
        {
            checkpointManager.instance.checkpointPosition = new Vector3(162, -94, 0);
        }
        else if (checkpointToLoadTo == 1)
        {
            checkpointManager.instance.checkpointPosition = new Vector3(340, -242, 0);
        }

        //SceneManager.LoadScene(roomName);
        if (!fade.activeInHierarchy)
        {
            //Debug.Log("setting");
            if (audioHolder.activeInHierarchy)
            {
                audioHolder.SetActive(false);
            }
            fade.SetActive(true);
            StartCoroutine(delay());
        }
    }

    private IEnumerator delay ()
    {
        yield return new WaitForSeconds(fadeTime.length);
        SceneManager.LoadScene(roomName);
    }

}
