using UnityEngine;


[CreateAssetMenu(fileName = "NewNPCDialogue", menuName = "NPC Dialogue")]
public class NPCDialogue : ScriptableObject
{
    //to be implemeneted later
    /*
    public string npcName;
    public Sprite npcPortrait;
    public AudioSourceOrWhatever npcTalkSound;

    */

    [TextArea(1, 100)]
    public string[] dialogueList;
    public AudioClip sound;
}
