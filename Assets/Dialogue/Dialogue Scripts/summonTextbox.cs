using UnityEngine;

public class summonTextbox : MonoBehaviour
{

    public GameObject textBox;
    public NPCDialogue dialogue;

    private bool canTalk;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.E) && !textBox.activeInHierarchy) && canTalk)
        {
            textBox.GetComponent<Textbox>().index = 0;
            textBox.GetComponent<Textbox>().npcText = dialogue;
            textBox.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Guy"))
        {
            canTalk = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Guy"))
        {
            canTalk = false;
        }
    }

    /*
    private void OnTriggerStay2D(Collider2D collision)
    {
        //if NPC with dialogue if close to the player and they interact, summon textbox
        if(collision.gameObject.CompareTag("Guy"))
        {
            if ((Input.GetKeyDown(KeyCode.E) && !textBox.activeInHierarchy))
            {
                textBox.GetComponent<Textbox>().index = 0;
                textBox.GetComponent<Textbox>().npcText = dialogue;
                textBox.SetActive(true);
            }
        }
    }
    */

}
