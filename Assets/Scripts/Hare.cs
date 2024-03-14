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
        animator.SetBool("IsRunning", agent.desiredVelocity.magnitude > 0);
    }
}
