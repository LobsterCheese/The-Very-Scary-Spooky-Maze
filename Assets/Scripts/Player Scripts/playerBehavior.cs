using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class playerBehavior : MonoBehaviour
{
    [Header("Player Variables")]
    [SerializeField]
    private float playerSpeed = 2f;

    private bool jumped;

    [Header("Reference Objects")]
    [SerializeField]
    private GameObject jumpscareCanvas;
    [SerializeField]
    private GameObject jumpscareCanvasIMG;
    [SerializeField]
    private GameObject markerPrefab;
    [SerializeField]
    private RuntimeAnimatorController idle; //these should be refactored later
    [SerializeField]
    private RuntimeAnimatorController walking;
    [SerializeField]
    private GameObject dialogueBox;
    [SerializeField]
    private Camera cam;
    //public GameObject checkpoint1;
    [SerializeField]
    private GameObject PlayerUI;
    [SerializeField]
    private TextMeshProUGUI playerMarkersText;
    [SerializeField]
    private TextMeshProUGUI maxMarkersText;
    [SerializeField]
    private GameObject pauseMenu;

    private SpriteRenderer sprender;

    private Animator anim;
    //private Rigidbody2D rb;
    private bool canPlaceMarker;
    private bool canPlaceMarkerSafe;
    //private GameObject closestMarker;

    //for managing step sounds
    [SerializeField]
    private float stepDistance;
    private Vector3 lastLocation;
    private bool moved = false;

    private Vector2 mousePos;

    private float camSize;
    private float lerpSpeed = 0f;

    private Vector2 playerMovement;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        sprender = GetComponent<SpriteRenderer>();
        //rb = GetComponent<Rigidbody2D>();

        camSize = cam.orthographicSize;

        canPlaceMarker = true;

        //teleports player to checkpoint if they have made it to one
        if (checkpointManager.instance.madeIt || checkpointManager.instance.startingFromScreen)
        {
            transform.position = checkpointManager.instance.checkpointPosition;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //this sets players markers to the max 
        playerMarkersText.text = playerUpgrades.instance.currentMarkers.ToString();
        maxMarkersText.text = playerUpgrades.instance.markerAmount.ToString();

        //checks to make sure player is moving to play walking sound AFTER they have pressed any buttons
        if ((transform.position - lastLocation).magnitude > stepDistance && moved && playerUpgrades.instance.dontMove != 0f)
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
        if (Input.GetKeyDown(KeyCode.Space) && canPlaceMarker && playerUpgrades.instance.currentMarkers > 0 && !canPlaceMarkerSafe && playerUpgrades.instance.dontMove == 1f)
        {
            SFXManager.instance.PlaySoundRandom(SFXManager.instance.marker);
            playerUpgrades.instance.currentMarkers--;
            Instantiate(markerPrefab, transform.position, Quaternion.identity);
        }
        
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
        if(collision.gameObject.CompareTag("Spook") && !collision.gameObject.GetComponent<spookBehavior>().dying)
        {
            if(!jumpscareCanvas.activeInHierarchy)
            {
                if (!jumped)
                {
                    playerUpgrades.instance.deaths++;
                    SFXManager.instance.PlaySoundRandom(SFXManager.instance.spookJumpscare);
                    jumped = true;
                }
                jumpscareCanvas.GetComponent<jumpscareCanvas>().scaredBy = 0;
                jumpscareNT(collision);
            }
        }

        //if you run into happy henry, get jumpscared
        if (collision.gameObject.CompareTag("Happy"))
        {
            if (!jumpscareCanvas.activeInHierarchy)
            {
                if (!jumped)
                {
                    playerUpgrades.instance.deaths++;
                    SFXManager.instance.PlaySoundRandom(SFXManager.instance.henryJumpscare);
                    jumped = true;
                }
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
            playerUpgrades.instance.firstCheck = true;
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if you run into a skeleton that is currently moving, get jumpscared
        if (collision.gameObject.CompareTag("Skelly") && collision.gameObject.GetComponent<skellyNewBehavior>().moveMult != 0f)
        {
            if (!jumped)
            {
                playerUpgrades.instance.deaths++;
                SFXManager.instance.PlaySoundRandom(SFXManager.instance.skellyJumpscare);
                jumped = true;
            }
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
