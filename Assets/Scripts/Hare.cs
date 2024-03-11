using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Hare : MonoBehaviour
{
    NavMeshAgent agent;
    Animator animator;

    public void ReceiveDamage()
    {
        Debug.Log("Received hit");
    }

    void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("IsWalking", agent.desiredVelocity.magnitude > 0);
    }
    //void PickDestination(Vector2 initialDirection, float initialDistance)
    //{
    //    const int MaxIterations = 100;
    //    for (int i = 0; i < 100; i++)
    //    {
    //        if 
    //    }
    //}

    //bool SamplePosition(Vector3 position, out Vector3 result)
    //{
    //    result = Vector3.zero;
    //    if (NavMesh.SamplePosition(position, out NavMeshHit hit, .6f, NavMesh.AllAreas))
    //    {
    //        result = hit.position;
    //        NavMeshPath path = new();
    //        if (!(agent.CalculatePath(hit.position, path) && path.status == NavMeshPathStatus.PathComplete)) return false;

    //        return true;
    //    }
    //    return false;
    //}
}
