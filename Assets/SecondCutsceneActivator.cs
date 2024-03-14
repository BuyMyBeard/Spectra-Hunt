using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class SecondCutsceneActivator : MonoBehaviour
{
    bool activatedOnce = false;
    PlayableDirector director;
    [SerializeField] Beacon beacon;
    private void Awake()
    {
        director = GetComponentInChildren<PlayableDirector>();
        beacon = GetComponentInChildren<Beacon>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!activatedOnce)
        {
            activatedOnce = true;
            director.Play();
            beacon.gameObject.SetActive(false);
        }
    }
}
