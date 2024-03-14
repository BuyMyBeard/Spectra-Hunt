using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ForestHare : MonoBehaviour
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
    [SerializeField] float walkSpeed = 1.5f;
    [SerializeField] float walkTurnSpeed = 150;
    [SerializeField] float runTurnSpeed = 400;
    [SerializeField] float runSpeed = 10;
    [SerializeField] float safeRange;
    bool hasDetectedFox = false;
    [SerializeField] float updateDirectionTimer = 5;
    float timeSinceLastUpdatedDirection = 0;
    [SerializeField] float minDistanceToUpdateDirection = 5;

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
    private void Start()
    {
        UpdateDestination();
    }
    public void DetectFox()
    {
        if (hasDetectedFox) return;
        hasDetectedFox = true;
        StartCoroutine(CalmDown());
    }
    void Update()
    {
        animator.SetBool("IsRunning", agent.desiredVelocity.magnitude > 0 && hasDetectedFox);
        animator.SetBool("IsWalking", agent.desiredVelocity.magnitude > 0 && !hasDetectedFox);
        agent.speed = hasDetectedFox ? runSpeed : walkSpeed;
        agent.angularSpeed = hasDetectedFox ? runTurnSpeed : walkTurnSpeed;

        if (agent.remainingDistance < rallyPointRange)
        {
            if (currentDirection == Direction.Forward)
                currentIndex = NextIndex;
            else
                currentIndex = PreviousIndex;
        }
        if (hasDetectedFox && timeSinceLastUpdatedDirection > updateDirectionTimer 
            && Vector3.Distance(fox.transform.position, transform.position) < minDistanceToUpdateDirection)
        {
            timeSinceLastUpdatedDirection = 0;
            UpdateDestination();
        }
        else
            GoToNextDestination();
    }

    void GoToNextDestination()
    {
        RallyPoint before = rallyPoints[currentIndex];
        RallyPoint after = rallyPoints[NextIndex];
        if (currentDirection == Direction.Forward)
            currentObjective = after;
        else
            currentObjective = before;
        agent.SetDestination(currentObjective.transform.position);
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

    IEnumerator CalmDown()
    {
        yield return new WaitUntil(() => Vector3.Distance(fox.transform.position, transform.position) > safeRange);
        hasDetectedFox = false;
    }
}
