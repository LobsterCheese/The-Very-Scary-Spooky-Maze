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

    private SpriteRenderer sprender;

    private Animator anim;
    //private Rigidbody2D rb;
    private bool canPlaceMarker;
    private bool canPlaceMarkerSafe;
    private GameObject closestMarker;

    //for managing step sounds
    [SerializeField]
    private float stepDistance;
    private Vector3 lastLocation;
    private bool moved = false;

    private Vector2 mousePos;

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
        sprender = GetComponent<SpriteRenderer>();
        //rb = GetComponent<Rigidbody2D>();

        camSize = cam.orthographicSize;

        canPlaceMarker = true;
    }

    // Update is called once per frame
    void Update()
    {
        //this sets players markers to the max 
        playerMarkersText.text = playerUpgrades.instance.currentMarkers.ToString();
        maxMarkersText.text = playerUpgrades.instance.markerAmount.ToString();

        //maxMarkers = playerUpgrades.instance.markerAmount;

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

        //Debug.Log(((transform.position - lastLocation).magnitude > stepDistance));

        
        //checks to make sure player is moving to play walking sound AFTER they have pressed any buttons
        if ((transform.position - lastLocation).magnitude > stepDistance && moved)
        {
            lastLocation = transform.position;
            SFXManager.instance.PlaySoundRandom(SFXManager.instance.walking);
        }
        

        //this flips the character towards the direction their mouse is pointing in
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var mouseTemp = (mousePos - (Vector2)transform.position).normalized;
        //left
        if (mouseTemp.x < 0 && (mouseTemp.y < -1 || mouseTemp.y < 1))
        {
            sprender.flipX = true;
        }
        //right
        else if (mouseTemp.x >= 0 && (mouseTemp.y < -1 || mouseTemp.y < 1))
        {
            sprender.flipX = false;
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
                moved = true;
                anim.runtimeAnimatorController = walking;
            }
            else if (!Input.anyKey)
            {
                anim.runtimeAnimatorController = idle;
            }
        }

        //places a marker to keep track of where you have been
        if (Input.GetKeyDown(KeyCode.Space) && canPlaceMarker && playerUpgrades.instance.currentMarkers > 0 && !canPlaceMarkerSafe)
        {
            playerUpgrades.instance.currentMarkers--;
            Instantiate(markerPrefab, transform.position, Quaternion.identity);
        }

        /*
        //if there is already a marker here, return it to the player
        else if (Input.GetKeyDown(KeyCode.Space) && closestMarker != null)
        {
            marker++;
            Destroy(closestMarker);
        }
        */
        
        //gets player input
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

    //moves player
    void movePlayer(Vector2 direction)
    {
        transform.Translate(direction * playerSpeed * Time.deltaTime * playerUpgrades.instance.dontMove);
    }

    void FixedUpdate()
    {
        movePlayer(playerMovement);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if you run into a spook enemy, get jumpscared
        if(collision.gameObject.CompareTag("Spook"))
        {
            if(!jumpscareCanvas.activeInHierarchy)
            {
                jumpscareCanvas.GetComponent<jumpscareCanvas>().scaredBy = 0;
                jumpscareNT(collision);
            }
        }

        //if you run into happy henry, get jumpscared
        if (collision.gameObject.CompareTag("Happy"))
        {
            if (!jumpscareCanvas.activeInHierarchy)
            {
                jumpscareCanvas.GetComponent<jumpscareCanvas>().scaredBy = 2;
                jumpscareNT(collision);
            }
        }
        
        //if you are close to a marker, you cannot place another
        if (collision.gameObject.CompareTag("Marker"))
        {
            canPlaceMarker = false;
        }
        

        //if you reach the safe zone, set the checkpoint flag to true, player respawns from here after this
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

        //jolly jerry to be implemented later
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //if you are in the safe room, you cannot place markers
        if (collision.gameObject.CompareTag("Safe"))
        {
            canPlaceMarkerSafe = true;
            //also replenishes your markers, setting it to the max
            //maxMarkers = playerUpgrades.instance.markerAmount;
            //playerUpgrades.instance.currentMarkers = maxMarkers;

            playerUpgrades.instance.currentMarkers = playerUpgrades.instance.markerAmount;
        }

        if (collision.gameObject.CompareTag("Marker"))
        {
            canPlaceMarker = false;
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
    //sets a jumpscare if the colliding mob does not have a trigger (skelly & jolly jerry)
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
