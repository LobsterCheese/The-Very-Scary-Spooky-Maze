using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class deathScreenManager : MonoBehaviour
{
    [Header("References")]
    public TextMeshProUGUI monsterNameGUI;
    public TextMeshProUGUI monsterDescriptionGUI;
    public Image monsterFoundPhotoGUI;

    public jumpscareTemplate monster;

    public void OnEnable()
    {
        monsterNameGUI.text = monster.monsterName;
        monsterDescriptionGUI.text = monster.description;
        monsterFoundPhotoGUI.sprite = monster.monsterImg;
    }
}
