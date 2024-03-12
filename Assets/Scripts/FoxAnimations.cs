using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using Unity.Mathematics;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(FoxMovement))]

public class FoxAnimations : MonoBehaviour
{
    [SerializeField] Transform root, trapezius, thighL, thighR, shoulderL, shoulderR, neck, pivotPoint, targetRotation;
    [SerializeField] LayerMask groundLayer = 1;
    [SerializeField] float rootRotationSpeed = 300;

    [SerializeField] float neckRotationSpeed = 200;
    [SerializeField] float maxNeckDeltaAngle = 30;

    [SerializeField] float maxTurnAngleDelta = 90;
    [SerializeField] float maxBodyTurnAngle = 45;
    [SerializeField] float bodyTurnSpeed = 90;
    
    Animator animator;
    FoxMovement foxMovement;

    Quaternion rootRotationProgress;
    float neckRotationProgress;

    float localNeckRestingAngle;

    float bodyTurnProgress;

    Vector3 currentTranslationDiff = Vector3.zero;
    Quaternion currentRotationDiff = Quaternion.identity;

    Quaternion trapeziusRestingRotation;

    private void Awake()
    {
        foxMovement = GetComponent<FoxMovement>();
        animator = GetComponent<Animator>();
        targetRotation.parent = null;
        rootRotationProgress = root.rotation;
        localNeckRestingAngle = neck.localEulerAngles.x;
        neckRotationProgress = localNeckRestingAngle;
        trapeziusRestingRotation = trapezius.localRotation;
    }

    private void LateUpdate()
    {
        bool busy = animator.GetBool("IsBusy");
        if (busy)
        {
            rootRotationProgress = Quaternion.RotateTowards(rootRotationProgress, currentRotationDiff * root.rotation, rootRotationSpeed * Time.deltaTime);
            root.SetPositionAndRotation(currentTranslationDiff + root.position, rootRotationProgress);
            return;
        }
        TurnBody();

        bool hit1 = Physics.SphereCast(root.position, .05f, -transform.up, out RaycastHit backHit, 1f, groundLayer);
        bool hit2 = Physics.SphereCast(trapezius.position, .05f, -transform.up, out RaycastHit frontHit, 1f, groundLayer);

        float angle;

        if (!hit1 || !hit2) angle = 0;
        else
        {
            float l = Vector3.Distance(backHit.point, frontHit.point);
            float h = backHit.point.y - frontHit.point.y;
            angle = Mathf.Asin(h / l) * -Mathf.Rad2Deg;
        }   

        targetRotation.SetPositionAndRotation(root.position, root.rotation);
        targetRotation.RotateAround(pivotPoint.position, transform.right, -angle);

        root.GetPositionAndRotation(out Vector3 currentLocation, out Quaternion currentRotation);
        AdjustToInclineSmoothly();
        //
        // AdjustNeck();

        currentTranslationDiff = root.position - currentLocation;
        currentRotationDiff = root.rotation * Quaternion.Inverse(currentRotation);
    }

    void AdjustToInclineSmoothly()
    {
        rootRotationProgress = Quaternion.Euler(rootRotationProgress.eulerAngles.x, root.eulerAngles.y, root.eulerAngles.z);

        float slopeFactor = 1 + Mathf.Max(0, math.remap(30, 40, 0, 4, Mathf.Min(30, Quaternion.Angle(rootRotationProgress, targetRotation.rotation))));

        rootRotationProgress = Quaternion.RotateTowards(rootRotationProgress, targetRotation.rotation, rootRotationSpeed * Time.deltaTime * slopeFactor);
        root.SetPositionAndRotation(targetRotation.position, rootRotationProgress);
    }

    void AdjustNeck()
    {
        if (Mathf.Abs(neck.localEulerAngles.x - localNeckRestingAngle) < 10) return; 
        neckRotationProgress = Mathf.MoveTowardsAngle(neckRotationProgress, localNeckRestingAngle, neckRotationSpeed);
        neckRotationProgress = Extensions.ClampAngle(neckRotationProgress, neck.eulerAngles.x - maxNeckDeltaAngle, neck.eulerAngles.x + maxNeckDeltaAngle);
        neck.rotation = Quaternion.Euler(neckRotationProgress, neck.eulerAngles.y, neck.eulerAngles.z);
    }
    void TurnBody()
    {
        trapezius.localRotation = trapeziusRestingRotation;
        Vector3 foxDirection = transform.forward;
        foxDirection.y = 0;
        Vector3 movementDirection = foxMovement.MovementDirection;
        movementDirection.y = 0;

        float turnDirection = -Mathf.Sign(Vector3.Cross(foxDirection, movementDirection).y);

        float deltaAngle = Vector3.Angle(foxDirection, movementDirection);

        float turnAngle = turnDirection * Mathf.Min(maxTurnAngleDelta, deltaAngle) / (maxTurnAngleDelta / maxBodyTurnAngle);

        bodyTurnProgress = Mathf.MoveTowardsAngle(bodyTurnProgress, turnAngle, bodyTurnSpeed * Time.deltaTime);

        trapezius.Rotate(Vector3.up * bodyTurnProgress);
    }
}
