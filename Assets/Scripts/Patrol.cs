using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Hare))]
[RequireComponent(typeof(NavMeshAgent))]
public class Patrol : MonoBehaviour
{
    [SerializeField] Task[] tasks;
    Animator animator;
    NavMeshAgent agent;
    [SerializeField] float minRemainingDistance = .1f;

    int currentTaskIndex = 0;

    [Serializable]
    public struct Task
    {
        public PatrolPoint patrolPoint;
        public float waitTime;
    }
    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    [ContextMenu("Start Patrolling")]
    public void StartPatrolling() => StartCoroutine(nameof(PatrolAround));

    [ContextMenu("Stop Patrolling")]
    public void StopPatrolling() => StopCoroutine(nameof(PatrolAround));
    IEnumerator PatrolAround()
    {
        while (true)
        {
            Task currentTask = tasks[currentTaskIndex];
            agent.SetDestination(currentTask.patrolPoint.transform.position);
            yield return null;
            yield return new WaitUntil(() => agent.remainingDistance < minRemainingDistance);
            agent.ResetPath();
            if (currentTask.waitTime < 0) break;
            if (currentTask.waitTime >= 3) animator.SetTrigger("Eat");
            yield return new WaitForSeconds(currentTask.waitTime);
            currentTaskIndex = (currentTaskIndex + 1) % tasks.Length;
        }
    }

    [ContextMenu("Test")]
    public void GoToFirst()
    {
        InvokeRepeating(nameof(Go), 1, .5f);
    }
    public void Go()
    {
        agent.SetDestination(tasks[0].patrolPoint.transform.position);
    }
}
