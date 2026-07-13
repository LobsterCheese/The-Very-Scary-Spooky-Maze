using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class restart : MonoBehaviour
{

    private bool restarting;

    [SerializeField]
    private AnimationClip clip;

    [SerializeField]
    private GameObject img;

    private IEnumerator delayFade(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !restarting)
        {
            if (!img.activeInHierarchy)
            {
                SFXManager.instance.PlaySoundRandom(SFXManager.instance.bellClick);
                img.SetActive(true);
            }
            restarting = true;
            StartCoroutine(delayFade(clip.length));
            playerUpgrades.instance.currentMarkers = playerUpgrades.instance.markerAmount;
        }
    }
}
