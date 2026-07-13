using UnityEngine;
using UnityEngine.Rendering.Universal;

public class flashlightFlicker : MonoBehaviour
{

    //how long the flashlight can stay on for 
    [SerializeField]
    private float on;
    //the maximum life of flashlight (just used for reference so setting flashlight hp is easier)
    private float maxFlashLife;

    private Light2D beam;
    [SerializeField]
    private PolygonCollider2D hitbox;

    private float alpha;
    private bool flashing;

    private bool forceOff = false;

    //how long it takes for the flashlight to turn back on if it shuts off
    [SerializeField]
    private float resetTimer;
    //turns flashlight back on
    private float reset;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        beam = GetComponent<Light2D>();
        hitbox = GetComponent<PolygonCollider2D>();
        maxFlashLife = on;
        forceOff = false;
    }

    // Update is called once per frame
    void Update()
    {
        //if player presses button and has unlocked upgrade, they will turn the flashlight off
        if (Input.GetMouseButtonDown(0) && !forceOff && hitbox.enabled && playerUpgrades.instance.onAndOff)
        {
            SFXManager.instance.PlaySoundRandom(SFXManager.instance.flashlight);
            hitbox.enabled = !hitbox.enabled;
            beam.intensity = 0;
        }
        //if player presses button and has unlocked upgrade, they will turn flashlight on
        else if (Input.GetMouseButtonDown(0) && !forceOff && !hitbox.enabled && playerUpgrades.instance.onAndOff)
        {
            SFXManager.instance.PlaySoundRandom(SFXManager.instance.flashlight);
            hitbox.enabled = !hitbox.enabled;
            beam.intensity = 1;
        }

        //if flashlight life is greater than max, it stops filling up to prevent overflow
        if(on >= maxFlashLife && !forceOff)
        {
            on = maxFlashLife;
            forceOff = false;
        }

        //if flashlight runs out of life, turn off, player cannot turn it back on until it's fully charged
        if(on < 0)
        {
            forceOff = true;
        }

        //if flashlight is not flickering, it is recharging
        if (!flashing && !forceOff)
        {
            on += Time.deltaTime;
        }

        //when flashlight is flickering, drain life and randomize opacity
        if (flashing)
        {
            alpha = Random.Range(0.2f, 1f);
            beam.intensity = alpha;
            on -= 2.2f * Time.deltaTime;
        }

        //turns off collider and light, then recharges
        if (forceOff)
        {
            beam.intensity = 0;
            hitbox.enabled = false;
            reset += Time.deltaTime;

            if(reset > maxFlashLife)
            {
                reset = 0;
                on = maxFlashLife;
                beam.intensity = 1;
                hitbox.enabled = true;
                forceOff = false;
            }
        }

    }

    //if flashlight is on happy henry, it will start flickering
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Happy") && !forceOff)
        {
            flashing = true;
        }
    }

    //if flashlight is taken off happy henry and still has charge, it goes back to normal
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Happy"))
        {
            flashing = false;
            beam.intensity = 1;
        }
    }
}
