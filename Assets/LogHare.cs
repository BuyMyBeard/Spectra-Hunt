using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LogHare : MonoBehaviour
{
    [SerializeField] PatrolPoint hideSpot;
    Patrol patrol;
    NavMeshAgent agent;
    Animator animator;
    bool hasDetectedFox = false;
    [SerializeField] float walkSpeed = 1.5f;
    [SerializeField] float walkTurnSpeed = 150;
    [SerializeField] float runTurnSpeed = 400;
    [SerializeField] float runSpeed = 10;
    Transform fox;
    [SerializeField] float safeRange;
    private void Awake()
    {
        patrol = GetComponent<Patrol>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        fox = FindObjectOfType<FoxMovement>().transform;
    }

    private void Start()
    {
        patrol.StartPatrolling();
    }

    public void DetectFox()
    {
        if (hasDetectedFox) return;
        patrol.StopPatrolling();
        hasDetectedFox = true;
        agent.SetDestination(hideSpot.transform.position);
        StartCoroutine(GetDownOnceFoxLeaves());
    }
    void Update()
    {
        animator.SetBool("IsRunning", agent.desiredVelocity.magnitude > 0 && hasDetectedFox);
        animator.SetBool("IsWalking", agent.desiredVelocity.magnitude > 0 && !hasDetectedFox);
        agent.speed = hasDetectedFox ? runSpeed : walkSpeed;
        agent.angularSpeed = hasDetectedFox ? runTurnSpeed : walkTurnSpeed;
    }

    IEnumerator GetDownOnceFoxLeaves()
    {
        yield return new WaitUntil(() => Vector3.Distance(fox.position, transform.position) > safeRange);
        hasDetectedFox = false;
        patrol.StartPatrolling();
    }
}
