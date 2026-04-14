using UnityEngine;
using UnityEngine.Rendering.Universal;

public class debugLight : MonoBehaviour
{

    public Light2D dark;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    // Update is called once per frame
    void Update()
    {
        //turns off the darkness so you can see everything
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if(dark.intensity != 0.5f)
            {
                dark.intensity = 0.5f;
            }
            else
            {
                dark.intensity = 0f;
            }
        }
    }
}
