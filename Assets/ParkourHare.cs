using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ParkourHare : MonoBehaviour
{
    Animator animator;
    NavMeshAgent agent;
    Transform fox;
    [SerializeField] PatrolPoint topOfRock;
    [SerializeField] PatrolPoint bottomOfRock;
    [SerializeField] float runSpeed = 3;
    bool runningAway = false;
    [SerializeField] float turnSpeed = 300;
    [SerializeField] float runAwayDistance = 25;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        fox = FindObjectOfType<FoxMovement>().transform;
    }
    public void GoToTopOfRock()
    {
        agent.SetDestination(topOfRock.transform.position);
        StartCoroutine(TurnAfterReachingTop());
    }
    IEnumerator TurnAfterReachingTop()
    {
        yield return null;
        yield return new WaitUntil(() => agent.remainingDistance < .5f);
        Quaternion goal = Quaternion.LookRotation(-Vector3.forward);

        for(int i = 0; i < 10000; i++)
        {
            if (transform.rotation == goal) yield break;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, goal, Time.deltaTime * turnSpeed);
        }
    }

    private void Update()
    {
        animator.SetBool("IsRunning", runningAway || agent.desiredVelocity.magnitude > 0);
    }

    public void DropFromRock()
    {
        agent.SetDestination(bottomOfRock.transform.position);
        StartCoroutine(RunAwayAfterDropping());
    }

    IEnumerator RunAwayAfterDropping()
    {
        yield return null;
        yield return new WaitUntil(() => agent.remainingDistance < .5f);
        agent.isStopped = true;
        runningAway = true;
        while (true)
        {
            if (Vector3.Distance(fox.position, transform.position) < runAwayDistance)
            {
                Vector3 opposite = transform.position - fox.position;
                opposite.y = 0;
                opposite = opposite.normalized;
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(opposite), turnSpeed * Time.deltaTime);
                agent.Move(runSpeed * Time.deltaTime * transform.forward);
                yield return null;
            }
        }
    }

    
}
