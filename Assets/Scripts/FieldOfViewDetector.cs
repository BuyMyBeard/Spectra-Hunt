using System.Collections;
using UnityEngine;
using Unity.Mathematics;
using UnityEngine.AI;
using UnityEngine.Events;

public class FieldOfViewDetector : MonoBehaviour
{
    const float TimeBetweenChecks = .2f;
    [SerializeField] Transform eyes;
    [SerializeField] LayerMask environmentMask;
    [SerializeField] UnityEvent detectPlayer = new();
    [SerializeField] Transform detectTarget;

    [SerializeField] float viewRange = 40f;
    
    [Range(0f, 90f)]
    [SerializeField] float viewAngle = 90f;
    [SerializeField] float timeToDetect = .6f;
    [SerializeField] float timeToForget = 10;
    [SerializeField] bool ignoreIfUnreachable = true;
    NavMeshAgent agent;
    float timeInSight = 0;
    public float DotViewAngle { get => math.remap(0, 180, 1, 0, viewAngle); }
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        StartCoroutine(DetectPlayer());
    }
    bool IsPlayerSighted
    {
        get
        {
            float distanceToTarget = Vector3.Distance(eyes.position, detectTarget.position);
            Vector3 directionToTarget = detectTarget.position - eyes.position;

            return (IsInViewAngle(directionToTarget) && TargetInRangeAndSight(directionToTarget, distanceToTarget));
        }
    }
    bool CanReachTarget
    {
        get
        {
            NavMeshPath path = new();
            return agent.CalculatePath(detectTarget.position, path) && path.status == NavMeshPathStatus.PathComplete;
        }
    }
    bool IsInViewAngle(Vector3 directionToTarget)
    {
        Vector2 flatDelta = new(directionToTarget.x, directionToTarget.z);
        Vector2 flatEnemyForward = new(eyes.forward.x, eyes.forward.z);
        float dot = Vector2.Dot(flatEnemyForward.normalized, flatDelta.normalized);
        return dot > DotViewAngle;
    }
    bool TargetInRangeAndSight(Vector3 directionToTarget, float distanceToTarget) => distanceToTarget < viewRange && !Physics.Raycast(eyes.position, directionToTarget, distanceToTarget, environmentMask);

    IEnumerator DetectPlayer()
    {
        while (true)
        {
            yield return new WaitForSeconds(TimeBetweenChecks);
            if (IsPlayerSighted && (!ignoreIfUnreachable || CanReachTarget))
            {
                timeInSight += TimeBetweenChecks;
                if (timeInSight >= timeToDetect)
                    detectPlayer.Invoke();
            }
            else
                timeInSight = 0;
        }
    }
}
