using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StartCutscene : MonoBehaviour
{
    [SerializeField] CutsceneHare hare;
    [SerializeField] Transform[] locations;
    int index = -1;
    Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    [ContextMenu("Start Cutscene")]
    public void Play()
    {
        animator.SetTrigger("StartCutscene");
    }
    public void SendToNextLocation()
    {
        index = (index + 1) % locations.Length;
        hare.GoToLocation(locations[index].position);
    }
    public void WarpToNextLocation()
    {
        index = (index + 1) % locations.Length;
        hare.Warp(locations[index].position);
    }
    public void Eat()
    {
        hare.Eat();
    }
}
