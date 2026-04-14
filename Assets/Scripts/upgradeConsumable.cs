using UnityEngine;

public class upgradeConsumable : MonoBehaviour
{

    //set variable in accordance to which stat this item upgrades
    public bool speed;
    public bool str;
    public bool size;

    private bool upgraded;

    //fill in how much you want stats to increase
    public float upgradeAmount;

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
            if (speed)
            {
                playerUpgrades.instance.playerSpeed += upgradeAmount;
            }
            if (str)
            {
                playerUpgrades.instance.flashlightStr += upgradeAmount;
            }
            if (size)
            {
                playerUpgrades.instance.flashlightSize += new Vector3(upgradeAmount, upgradeAmount, 0);
            }

            Destroy(gameObject);
        }
    }

}
