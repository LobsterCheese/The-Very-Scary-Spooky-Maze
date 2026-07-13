using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class unpauseManager : MonoBehaviour
{
    [Header("everything")]
    //private RuntimeAnimatorController controller;
    private Animator anim;
    [SerializeField]
    private RuntimeAnimatorController unpauseStart;
    [SerializeField]
    private RuntimeAnimatorController pauseStart;
    [SerializeField]
    private AnimationClip unpauseClip;
    private float clipLength;

    [Header("all the fade stuff")]
    [SerializeField]
    private GameObject fade;
    [SerializeField]
    private AnimationClip fadeClip;

    //for turning off parent
    [SerializeField]
    private GameObject parent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //controller = GetComponent<RuntimeAnimatorController>();
        anim = GetComponent<Animator>();
        clipLength = unpauseClip.length;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //controller = unpauseStart;
            if (Time.timeScale == 0)
            {
                //Debug.Log("it's 0 alright so i should be turning off");
                SFXManager.instance.PlaySoundRandom(SFXManager.instance.pause);
                anim.runtimeAnimatorController = unpauseStart;
                Time.timeScale = 1.0f;
                StartCoroutine(turnOff());
                playerUpgrades.instance.dontMove = 1f;
            }
        }
    }

    public void buttonVersion()
    {
        //controller = unpauseStart;
        if (Time.timeScale == 0)
        {
            SFXManager.instance.PlaySoundRandom(SFXManager.instance.bellClick);
            //Debug.Log("it's 0 alright so i should be turning off");
            anim.runtimeAnimatorController = unpauseStart;
            Time.timeScale = 1.0f;
            //playerUpgrades.instance.dontMove = 1f;
            StartCoroutine(turnOff());
            playerUpgrades.instance.dontMove = 1f;
        }
    }

    public void quitButtonVersion()
    {
        //controller = unpauseStart;
        if (Time.timeScale == 0)
        {
            if (!fade.activeInHierarchy)
            {
                fade.SetActive(true);
            }
            SFXManager.instance.PlaySoundRandom(SFXManager.instance.bellClick);
            //Debug.Log("it's 0 alright so i should be turning off");
            anim.runtimeAnimatorController = unpauseStart;
            Time.timeScale = 1.0f;
            //playerUpgrades.instance.dontMove = 1f;
            StartCoroutine(getOut());
            playerUpgrades.instance.dontMove = 0f;
        }
    }

    private void OnDisable()
    {
        anim.runtimeAnimatorController = pauseStart;
        //controller = pauseStart;
    }

    IEnumerator getOut()
    {
        yield return new WaitForSeconds(fadeClip.length);
        SceneManager.LoadScene("startScreen");
    }

    IEnumerator turnOff()
    {
        //Debug.Log("happening");
        yield return new WaitForSeconds(clipLength);
        parent.SetActive(false);
    }
}
