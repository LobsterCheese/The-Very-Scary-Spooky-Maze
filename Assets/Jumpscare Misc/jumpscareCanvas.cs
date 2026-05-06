using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class jumpscareCanvas : MonoBehaviour
{
    //how long the jumpscare plays
    [SerializeField]
    private float duration = 2f;
    private Canvas jumpCanvas;

    public jumpscareTemplate[] jumpscareInfo;
    public int scaredBy;
    /*
     * 0 is spook
     * 1 is skelly
     * 2 is happy henry
     * 3 is jolly jerry
     * default to spook if somehow it doesnt trigger
     */

    public GameObject deathPrefab;
    public GameObject display;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        jumpCanvas = GetComponent<Canvas>();
    }

    private void OnEnable()
    {
        //loads corresponding info into the death screen
        deathPrefab.GetComponent<deathScreenManager>().monster = jumpscareInfo[scaredBy];
        display.GetComponent<Animator>().runtimeAnimatorController = jumpscareInfo[scaredBy].animController;
    }

    // Update is called once per frame
    void Update()
    {
        duration -= Time.deltaTime;

        if (jumpCanvas.isActiveAndEnabled && duration < 0)
        {
            StartCoroutine(deathScreen());
        }
    }

    IEnumerator deathScreen()
    {
        if (!deathPrefab.activeInHierarchy)
        {
            deathPrefab.SetActive(true);
            yield return new WaitForSeconds(duration);
        }
    }

}
