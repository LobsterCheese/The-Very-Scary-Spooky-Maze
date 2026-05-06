using UnityEngine;

public class upgradeConsumable : MonoBehaviour
{
    //fill in how much you want stats to increase
    [SerializeField]
    private int upgradeAmount;

    [SerializeField]
    private bool onAndOff;

    private void Awake()
    {
        if (playerUpgrades.instance.checkpoint)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("happening");
        //if player picks up consumable, upgrade corresponding stat and then destroy self
        if (collision.gameObject.CompareTag("Guy"))
        {
            playerUpgrades.instance.markerAmount += upgradeAmount;
            if (onAndOff)
            {
                playerUpgrades.instance.onAndOff = true;
            }
            Destroy(gameObject);
        }
    }

}
