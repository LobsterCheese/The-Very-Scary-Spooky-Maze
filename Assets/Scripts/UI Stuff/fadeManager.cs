using System.Collections;
using UnityEngine;
//using static UnityEditor.Experimental.GraphView.GraphView;

public class fadeManager : MonoBehaviour
{

    [SerializeField]
    private AnimationClip clip;

    private void Start()
    {
        StartCoroutine(delayOff());
    }

    private IEnumerator delayOff()
    {
        yield return new WaitForSeconds(clip.length + (clip.length*0.1f));
        gameObject.SetActive(false);
    }

}
