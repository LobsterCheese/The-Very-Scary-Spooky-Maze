using UnityEngine;

public class flashlightBeamManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = playerUpgrades.instance.flashlightSize;
    }
}
