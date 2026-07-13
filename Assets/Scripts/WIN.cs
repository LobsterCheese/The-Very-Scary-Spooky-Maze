using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class WIN : MonoBehaviour
{

    [SerializeField]
    private GameObject fade;

    [SerializeField]
    private AnimationClip fadeDuration;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Guy"))
        {
            if (!fade.activeInHierarchy)
            {
                playerUpgrades.instance.won = true;
                fade.SetActive(true);
                StartCoroutine(delayFade(fadeDuration.length));
            }
            //SceneManager.LoadScene("winScene");
        }
    }
    private IEnumerator delayFade(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("winScene");
    }

}
