using UnityEngine;
public class playerUpgrades : MonoBehaviour
{
    public static playerUpgrades instance;

    //for pausing player
    public float dontMove = 1f;
    //how big flashlight is
    public Vector3 flashlightSize = new Vector3(3f, 6f, 1f);
    //whether or not you can turn your flashlight on and off
    public bool onAndOff = false;

    //how many markers you can plaace
    public int markerAmount = 3;

    public bool checkpoint;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

    }
}
