using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using static UnityEngine.GraphicsBuffer;

public class monolithBehavior : MonoBehaviour
{
    //obelisk's rigidbodies
    private Rigidbody2D rb;
    [SerializeField]
    private GameObject solidRb;

    /*
    [SerializeField]
    private GameObject textbox;
    [SerializeField]
    private GameObject jumpscareCanvas;
    */

    //references flashlight
    [SerializeField]
    private Light2D beam;

    private SpriteRenderer sprender;

    private Animator anim;

    //has custom audio handling because of flashlight logic
    private GameObject target;
    private AudioSource source;
    [SerializeField]
    private float minDist;
    [SerializeField]
    private float maxDist;
    //0 should be asleep and 1 should be awake
    [SerializeField]
    private AudioClip[] clips;

    private bool onMe;

    [SerializeField]
    private RuntimeAnimatorController asleep;
    [SerializeField]
    private RuntimeAnimatorController awake;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        target = GameObject.Find("Guy");
        source = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sprender = GetComponent<SpriteRenderer>();

        source.clip = clips[0];
    }

    // Update is called once per frame
    void Update()
    {
        float dist = Vector3.Distance(transform.position, target.transform.position);

        //if the death screen is active, sound will be 0
        if (playerUpgrades.instance.dontMove == 0f)
        {
            source.volume = 0;
        }
        else if (dist < minDist)
        {
            source.volume = 0.5f;
        }
        else if (dist > maxDist)
        {
            source.volume = 0;
        }
        else
        {
            source.volume = 0.5f - ((dist - minDist) / (maxDist - minDist));
        }

        var tempAlpha = sprender.color;

        if (playerUpgrades.instance.onAndOff && beam.intensity == 0)
        {
            tempAlpha.a = 0.3f;
            sprender.color = tempAlpha;
            solidRb.SetActive(false);
            anim.runtimeAnimatorController = asleep;
        }
        else if (playerUpgrades.instance.onAndOff && beam.intensity != 0 && !onMe)
        {
            //turns back on but not awake
            tempAlpha.a = 1f;
            sprender.color = tempAlpha;
            anim.runtimeAnimatorController = asleep;
            if (!solidRb.activeInHierarchy)
            {
                solidRb.SetActive(true);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var tempAlpha = sprender.color;

        if (collision.gameObject.tag == "Flashlight" || (collision.gameObject.tag == "Marker"))
        {

            onMe = true;

            //plays awake sound and becomes impassable
            if (source.clip != clips[1])
            {
                source.clip = clips[1];
            }
            source.Play();
            tempAlpha.a = 1f;
            sprender.color = tempAlpha;
            anim.runtimeAnimatorController = awake;
            if (!solidRb.activeInHierarchy)
            {
                solidRb.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        var tempAlpha = sprender.color;

        if (collision.gameObject.tag == "Flashlight" || (collision.gameObject.tag == "Marker"))
        {

            onMe = false;

            //plays asleep sound and becomes impassable
            if (source.clip != clips[0])
            {
                source.clip = clips[0];
            }
            source.Play();
            tempAlpha.a = 1f;
            sprender.color = tempAlpha;
            anim.runtimeAnimatorController = asleep;
            if (!solidRb.activeInHierarchy)
            {
                solidRb.SetActive(true);
            }
        }
    }

}
