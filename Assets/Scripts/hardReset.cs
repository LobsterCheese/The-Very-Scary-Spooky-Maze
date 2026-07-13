using UnityEngine;

public class hardReset : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerUpgrades.instance.won)
        {
            checkpointManager.instance.checkpointUpgrades[0] = false;
            checkpointManager.instance.checkpointUpgrades[1] = false;
            playerUpgrades.instance.markerAmount = 3;
            playerUpgrades.instance.onAndOff = false;
            playerUpgrades.instance.won = false;
        }
    }
}
