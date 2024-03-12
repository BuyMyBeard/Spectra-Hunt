using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
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

    int NextIndex => (currentIndex + 1) % rallyPoints.Length;
    int PreviousIndex => currentIndex - 1 < 0 ? rallyPoints.Length - 1 : currentIndex - 1;
    
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        rallyPoints = rallyPointsParent.GetComponentsInChildren<RallyPoint>();
        fox = FindObjectOfType<FoxMovement>();
        animator = GetComponent<Animator>();
        currentIndex = rallyPoints.Length - 1;
    }
    [ContextMenu("Run")]
    void StartRunning()
    {
        StartCoroutine(RaceAround());
    }

    private void Update()
    {
        animator.SetBool("IsRunning", agent.desiredVelocity.magnitude != 0);
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
        while (true)
        {
            UpdateDestination();
            yield return new WaitForSeconds(.3f);
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
