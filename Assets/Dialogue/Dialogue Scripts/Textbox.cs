using TMPro;
using UnityEngine;
using System.Collections;

public class Textbox : MonoBehaviour
{
    [Header("References")]
    public NPCDialogue npcText;
    public TextMeshProUGUI textDisplay;

    public int index = 0;
    private bool typing = true;
    //0 is fast, 1 is really really slow
    [SerializeField]
    private float typeSpeed = 0.05f;

    //HAS NOT BEEN IMPLEMENTED
    [Header("section for if NPC has multiple sets of dialogue")]
    [SerializeField]
    private bool multipleDialogues;
    [SerializeField]
    private int NumofDialogues;

    //the coroutine that is currently running
    private Coroutine runningCo;

    private void OnEnable()
    {
        index = 0;
        typing = false;
        nextSentence();
    }

    void nextSentence()
    {
        if (index < npcText.dialogueList.Length)
        {
            //refreshes text to start writing next sentence
            textDisplay.text = "";
            runningCo = StartCoroutine(WriteSentence());
        }
        else
        {
            index = 0;
            npcText = null;
            gameObject.SetActive(false);
        }
    }

    IEnumerator WriteSentence()
    {
        foreach (char Character in npcText.dialogueList[index].ToCharArray())
        {
            textDisplay.text += Character;
            yield return new WaitForSeconds(typeSpeed);
        }
        index++;
        typing = true;
    }

    void nextSentenceSkip()
    {
        if (index < npcText.dialogueList.Length)
        {
            textDisplay.text = "";
            StartCoroutine(SkipSentence());
        }
        else
        {
            index = 0;
            textDisplay.text = "";
            gameObject.SetActive(false);
        }
    }

    IEnumerator SkipSentence()
    {
        StopCoroutine(runningCo);
        typing = true;
        textDisplay.text = "";
        textDisplay.text = npcText.dialogueList[index];
        yield return new WaitForSeconds(typeSpeed);
        index++;
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (typing)
            {
                //skips to the end of the sentence
                typing = false;
                nextSentence();
            }
            else if (!typing)
            {
                //goes to the next sentence
                nextSentenceSkip();
            }
        }
    }
}
