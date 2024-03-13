using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CutsceneHare : MonoBehaviour
{
    Animator animator;
    NavMeshAgent agent;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        animator.SetBool("IsWalking", agent.desiredVelocity.magnitude > 0);
    }
    public void GoToLocation(Vector3 location)
    {
        agent.SetDestination(location);
    }

    public void Eat()
    {
        animator.SetTrigger("Eat");
    }

    public void Warp(Vector3 location)
    {
        agent.Warp(location);
    }
}
