using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

[RequireComponent(typeof(Animator))]

public class FoxAnimations : MonoBehaviour
{
    [SerializeField] Transform root, trapezius, thighL, thighR, shoulderL, shoulderR, neck, pivotPoint, targetRotation;
    [SerializeField] LayerMask groundLayer = 1;
    Animator animator;
    [SerializeField] float rootRotationSpeed = 300;
    [SerializeField] float neckRotationSpeed = 200;

    Quaternion rootRotationProgress;
    float neckRotationProgress;

    float localNeckRestingAngle;
    [SerializeField] float maxNeckDeltaAngle = 30;

    Vector3 currentTranslationDiff = Vector3.zero;
    Quaternion currentRotationDiff = Quaternion.identity;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        targetRotation.parent = null;
        rootRotationProgress = root.rotation;
        localNeckRestingAngle = neck.localEulerAngles.x;
        neckRotationProgress = localNeckRestingAngle;
    }

    private void LateUpdate()
    {
        bool busy = animator.GetBool("Busy");
        if (busy)
        {
            rootRotationProgress = Quaternion.RotateTowards(rootRotationProgress, currentRotationDiff * root.rotation, rootRotationSpeed * Time.deltaTime);
            root.SetPositionAndRotation(currentTranslationDiff + root.position, rootRotationProgress);
            return;
        }


        bool hit1 = Physics.SphereCast(root.position, .05f, -transform.up, out RaycastHit backHit, .5f, groundLayer);
        bool hit2 = Physics.SphereCast(trapezius.position, .05f, -transform.up, out RaycastHit frontHit, .5f, groundLayer);
        if (!hit1 || !hit2) return;
        
        float l = Vector3.Distance(backHit.point, frontHit.point);
        float h = backHit.point.y - frontHit.point.y;
        float angle = Mathf.Asin(h / l) * -Mathf.Rad2Deg;

        targetRotation.SetPositionAndRotation(root.position, root.rotation);
        targetRotation.RotateAround(pivotPoint.position, transform.right, -angle);

        root.GetPositionAndRotation(out Vector3 currentLocation, out Quaternion currentRotation);
        AdjustToInclineSmoothly();
        AdjustNeck();

        currentTranslationDiff = root.position - currentLocation;
        currentRotationDiff = root.rotation * Quaternion.Inverse(currentRotation);

    }

    void AdjustToInclineSmoothly()
    {
        rootRotationProgress = Quaternion.Euler(rootRotationProgress.eulerAngles.x, root.eulerAngles.y, root.eulerAngles.z);
        rootRotationProgress = Quaternion.RotateTowards(rootRotationProgress, targetRotation.rotation, rootRotationSpeed * Time.deltaTime);
        root.SetPositionAndRotation(targetRotation.position, rootRotationProgress);
    }

    void AdjustNeck()
    {
        if (Mathf.Abs(neck.localEulerAngles.x - localNeckRestingAngle) < 10) return; 
        neckRotationProgress = Mathf.MoveTowardsAngle(neckRotationProgress, localNeckRestingAngle, neckRotationSpeed);
        neckRotationProgress = Extensions.ClampAngle(neckRotationProgress, neck.eulerAngles.x - maxNeckDeltaAngle, neck.eulerAngles.x + maxNeckDeltaAngle);
        neck.rotation = Quaternion.Euler(neckRotationProgress, neck.eulerAngles.y, neck.eulerAngles.z);
    }
}
