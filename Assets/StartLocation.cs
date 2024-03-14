using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartLocation : MonoBehaviour
{
    [SerializeField] Transform cutsceneFox;
    // Start is called before the first frame update
    void Start()
    {
        transform.SetPositionAndRotation(cutsceneFox.position, cutsceneFox.rotation);
        GetComponent<CameraMovement>().SyncFollowTarget();
    }
}
