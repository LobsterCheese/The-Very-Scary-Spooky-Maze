using UnityEngine;

public class upgradeConsumable : MonoBehaviour
{
    //fill in how much you want stats to increase
    [SerializeField]
    private int upgradeAmount;

    [SerializeField]
    private bool onAndOff;

    //if player has already made it to the checkpoint, delete this game object so it doesn't respawn
    private void Start()
    {
        if (playerUpgrades.instance.checkpoint)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if player picks up consumable, upgrade corresponding stat and then destroy self
        if (collision.gameObject.CompareTag("Guy"))
        {
            SFXManager.instance.PlaySoundRandom(SFXManager.instance.upgrade);
            //upgrades by whatever amount you fill in
            playerUpgrades.instance.markerAmount += upgradeAmount;
            //if on and off is ticked, this allows player to turn on and off the flashlight
            if (onAndOff)
            {
                playerUpgrades.instance.onAndOff = true;
            }
            Destroy(gameObject);
        }
    }

}
