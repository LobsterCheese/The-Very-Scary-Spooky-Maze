using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class playerBehavior : MonoBehaviour
{

    [Header("Player Variables")]
    public float playerSpeed = 2f;
    private int marker;
    private int maxMarkers;

    [Header("Reference Objects")]
    public GameObject jumpscareCanvas;
    public GameObject jumpscareCanvasIMG;
    public GameObject markerPrefab;
    public RuntimeAnimatorController idle; //these should be refactored later
    public RuntimeAnimatorController walking;
    public GameObject dialogueBox;
    public Camera cam;
    public GameObject checkpoint1;
    public GameObject PlayerUI;
    public TextMeshProUGUI playerMarkersText;
    public TextMeshProUGUI maxMarkersText;

    private Animator anim;
    //private Rigidbody2D rb;
    private bool canPlaceMarker;
    private bool canPlaceMarkerSafe;
    private GameObject closestMarker;

    private float camSize;
    private float lerpSpeed = 0f;

    private Vector2 playerMovement;

    private void Awake()
    {
        if (playerUpgrades.instance.checkpoint)
        {
            transform.position = checkpoint1.transform.position;
        }

        marker = playerUpgrades.instance.markerAmount;

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        //rb = GetComponent<Rigidbody2D>();

        camSize = cam.orthographicSize;

        canPlaceMarker = true;
    }

    // Update is called once per frame
    void Update()
    {
        playerMarkersText.text = marker.ToString();
        maxMarkersText.text = maxMarkers.ToString();

        maxMarkers = playerUpgrades.instance.markerAmount;

        //for testing and debugging purposes
        //resets scene in case you get stuck or want to retry
        /*
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        //restarts game from start screen
        if (Input.GetKeyDown(KeyCode.Escape))
        {

            SceneManager.LoadScene("startScreen");
        }
        */

        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        //flips player to face the camera
        if(mousePos.x < transform.localScale.x)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        //player control only works while textbox is not active in scene or not being jumpscared
        if (dialogueBox.activeInHierarchy || jumpscareCanvas.activeInHierarchy)
        {
            playerUpgrades.instance.dontMove = 0f;
        }
        else
        {
            playerUpgrades.instance.dontMove = 1f;
        }

        //forces player to idle if textbox is active in scene
        if (dialogueBox.activeInHierarchy)
        {
            anim.runtimeAnimatorController = idle;
        }

        //animation controller
        if (playerUpgrades.instance.dontMove != 0f)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
            {
                anim.runtimeAnimatorController = walking;
            }
            else if (!Input.anyKey)
            {
                anim.runtimeAnimatorController = idle;
            }
        }

        //places a marker to keep track of where you have been
        if (Input.GetKeyDown(KeyCode.Space) && canPlaceMarker && marker > 0 && !canPlaceMarkerSafe)
        {
            marker--;
            Instantiate(markerPrefab, transform.position, Quaternion.identity);
        }
        //if there is already a marker here, return it to the player
        else if (Input.GetKeyDown(KeyCode.Space) && !canPlaceMarker && !canPlaceMarkerSafe)
        {
            marker++;
            Destroy(closestMarker);
        }

        playerMovement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        //if you are within a safe zone, camera expands outwards and shrinks respectively
        if (canPlaceMarkerSafe)
        {
            lerpSpeed += Time.deltaTime;
            lerpSpeed = (lerpSpeed > 1f) ? 1f: lerpSpeed;
            cam.orthographicSize = Mathf.Lerp(camSize, camSize * 2, lerpSpeed);
        }
        else if (!canPlaceMarkerSafe)
        {
            lerpSpeed -= Time.deltaTime;
            lerpSpeed = (lerpSpeed < 0f) ? 0f : lerpSpeed;
            cam.orthographicSize = Mathf.Lerp(camSize, cam.orthographicSize, lerpSpeed);
        }
    }

    void movePlayer(Vector2 direction)
    {
        //if (!dialogueBox.activeInHierarchy || !jumpscareCanvas.activeInHierarchy) 
        //{ 
            transform.Translate(direction * playerSpeed * Time.deltaTime * playerUpgrades.instance.playerSpeed * playerUpgrades.instance.dontMove);
        //}
    }

    void FixedUpdate()
    {
        //if (!dialogueBox.activeInHierarchy || !jumpscareCanvas.activeInHierarchy)
        {

            movePlayer(playerMovement);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if you run into a spook enemy, get jumpscared
        if(collision.gameObject.CompareTag("Spook"))
        {
            if(!jumpscareCanvas.activeInHierarchy)
            {
                PlayerUI.SetActive(false);
                jumpscareCanvas.GetComponent<jumpscareCanvas>().scaredBy = 0;
                jumpscareNT(collision);
            }
        }

        if (collision.gameObject.CompareTag("Happy"))
        {
            if (!jumpscareCanvas.activeInHierarchy)
            {
                PlayerUI.SetActive(false);
                jumpscareCanvas.GetComponent<jumpscareCanvas>().scaredBy = 2;
                jumpscareNT(collision);
            }
        }

        //if you are close to a marker, you cannot place another
        if (collision.gameObject.CompareTag("Marker"))
        {
            canPlaceMarker = false;
            closestMarker = collision.gameObject;
        }

        if (collision.gameObject.CompareTag("Safe"))
        {
            playerUpgrades.instance.checkpoint = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if you run into a skeleton that is currently moving, get jumpscared
        if (collision.gameObject.CompareTag("Skelly") && collision.gameObject.GetComponent<spookBehavior>().currentSpeed != 0)
        {
            jumpscareCanvas.GetComponent<jumpscareCanvas>().scaredBy = 1;
            jumpscareTrigger(collision);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //if you are in the safe room, you cannot place markers
        if (collision.gameObject.CompareTag("Safe"))
        {
            canPlaceMarkerSafe = true;
            //also replenishes your markers
            maxMarkers = playerUpgrades.instance.markerAmount;
            marker = maxMarkers;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //if you move away from a marker or exit the safe zone, you can place markers again
        if (collision.gameObject.CompareTag("Marker"))
        {
            canPlaceMarker = true;
        }

        if (collision.gameObject.CompareTag("Safe"))
        {
            canPlaceMarkerSafe = false;
        }
    }

    //this section is dedicated to the methods that summon the proper UI for when jumpscares are activated
    //sets a jumpscare if the colliding mob does not have a trigger (skelly)
    private void jumpscareTrigger(Collision2D collision)
    {
        if (!jumpscareCanvas.activeInHierarchy)
        {
            PlayerUI.SetActive(false);
            jumpscareCanvas.SetActive(true);
        }
    }

    //sets a jumpscare if the colliding mob has a trigger (happy henry & spook)
    private void jumpscareNT(Collider2D collision)
    {
        if (!jumpscareCanvas.activeInHierarchy)
        {
            PlayerUI.SetActive(false);
            jumpscareCanvas.SetActive(true);
        }
    }
}
