using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RaceHare : MonoBehaviour
{
    enum Direction { Forward, Backward }
    [SerializeField] Transform rallyPointsParent;
    [SerializeField] float rallyPointRange = 3;
    RallyPoint[] rallyPoints;
    NavMeshAgent agent;
    FoxMovement fox;
    // index 0 means between 0 and 1
    int currentIndex;
    RallyPoint currentObjective;
    Direction currentDirection;
    Animator animator;
    [SerializeField] float startRunningDistance = 15;
    [SerializeField] float stopRunningDistance = 100;
    bool isRacing = false;

    int NextIndex => (currentIndex + 1) % rallyPoints.Length;
    int PreviousIndex => currentIndex - 1 < 0 ? rallyPoints.Length - 1 : currentIndex - 1;
    
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        rallyPoints = rallyPointsParent.GetComponentsInChildren<RallyPoint>();
        fox = FindObjectOfType<FoxMovement>(true);
        animator = GetComponent<Animator>();
        currentIndex = rallyPoints.Length - 1;
    }

    private void Update()
    {
        animator.SetBool("IsRunning", agent.desiredVelocity.magnitude != 0);
        float distance = Vector3.Distance(fox.transform.position, transform.position);

        if (isRacing && distance > stopRunningDistance)
        {
            StopAllCoroutines();
            isRacing = false;
            agent.destination = transform.position;
        }
        else if (!isRacing && distance < startRunningDistance)
        {
            StartCoroutine(RaceAround());
        }
    }

    void UpdateDestination()
    {
        RallyPoint before = rallyPoints[currentIndex];
        RallyPoint after = rallyPoints[NextIndex];

        Vector3 b = after.transform.position - before.transform.position;
        Vector3 a = (transform.position - fox.transform.position);

        if (Vector3.Dot(a, b) < 0)
        {
            currentDirection = Direction.Backward;
            currentObjective = before;
        }
        else
        {
            currentDirection = Direction.Forward;
            currentObjective = after;
        }

        agent.SetDestination(currentObjective.transform.position);
    }

    IEnumerator RaceAround()
    {
        isRacing = true;
        while (true)
        {
            UpdateDestination();
            yield return new WaitForSeconds(.1f);
            if (agent.remainingDistance < rallyPointRange)
            {
                if (currentDirection == Direction.Forward)
                    currentIndex = NextIndex;
                else
                    currentIndex = PreviousIndex;
            }
        }
    }
}
