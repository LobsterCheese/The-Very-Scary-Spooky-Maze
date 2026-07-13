using UnityEngine;

public class pauseManager : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseCanvas;
    [SerializeField]
    private GameObject textbox;
    [SerializeField]
    private GameObject deathScreen;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        //Debug.Log(Time.timeScale);

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            //cannot press escape while either or these are open
            if (!deathScreen.activeInHierarchy && !textbox.activeInHierarchy)
            {
                //if time is unpaused
                if (Time.timeScale != 0)
                {
                    //Debug.Log("time scale isnt 0");
                    //to implement audio later
                    if (!pauseCanvas.activeInHierarchy)
                    {
                        //Debug.Log("and the pause canvas aint active");
                        SFXManager.instance.PlaySoundRandom(SFXManager.instance.pause);
                        pauseCanvas.SetActive(true);
                        Time.timeScale = 0;
                        playerUpgrades.instance.dontMove = 0f;
                    }
                }
            }
        }
    }
}
