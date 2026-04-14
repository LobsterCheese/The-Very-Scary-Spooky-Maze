using UnityEngine;

[CreateAssetMenu(fileName = "jumpscareTemplate", menuName = "Jumpscare Info")]

public class jumpscareTemplate : ScriptableObject
{
    //name of monster & its description
    public string monsterName;
    public string description;

    //image for monster
    public Sprite monsterImg;
}
