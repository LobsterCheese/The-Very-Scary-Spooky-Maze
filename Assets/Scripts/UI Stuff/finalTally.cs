using TMPro;
using UnityEngine;

public class finalTally : MonoBehaviour
{

    private TextMeshProUGUI mesh;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mesh = GetComponent<TextMeshProUGUI>();
        if(playerUpgrades.instance.deaths == 0)
        {
            mesh.text = "You didn't die a single time!";
        }
        if(playerUpgrades.instance.deaths == 1)
        {
            mesh.text = "You only died once!";
        }
        else if (playerUpgrades.instance.deaths != 0)
        {
            mesh.text = "You only died " + playerUpgrades.instance.deaths.ToString() + " times!";
        }

        //resets deaths
        playerUpgrades.instance.deaths = 0;

    }
}
