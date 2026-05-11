using UnityEngine;

public class checkpointManager : MonoBehaviour
{
    public static checkpointManager instance;

    public int currentCheckpoint;
    public bool madeIt;

    public Vector3 checkpointPosition;

    private void Awake()
    {
        if (instance == null)
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
