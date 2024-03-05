using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

[RequireComponent(typeof(Animator))]

public class FoxAnimations : MonoBehaviour
{
    [SerializeField] Transform root, thighL, thighR, shoulderL, shoulderR, neck, pivotPoint, targetRotation;
    [SerializeField] LayerMask groundLayer = 1;
    Animator animator;
    [SerializeField] float maxDistanceDelta = 60;
    [SerializeField] float maxDegreesDelta = 2;

    Vector3 positionProgress;
    Quaternion rotationProgress;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        targetRotation.parent = null;
        positionProgress = root.position;
        rotationProgress = root.rotation;
    }

    private void LateUpdate()
    {
        bool busy = animator.GetBool("Busy");
        if (busy)
        {
            AdjustToInclineSmoothly();
            return;
        }
        Vector3 thighOrigin = (thighL.position + thighR.position) / 2;
        Vector3 shoulderOrigin = (shoulderL.position + shoulderR.position) / 2;

        bool hit1 = Physics.Raycast(thighOrigin, -transform.up, out RaycastHit thighHit, .5f, groundLayer);
        bool hit2 = Physics.Raycast(shoulderOrigin, -transform.up, out RaycastHit shoulderHit, .6f, groundLayer);
        if (!hit1 || !hit2) return;
        
        float l = Vector3.Distance(thighHit.point, shoulderHit.point);
        float h = thighHit.point.y - shoulderHit.point.y;
        float angle = Mathf.Asin(h / l) * -Mathf.Rad2Deg;

        targetRotation.SetPositionAndRotation(root.position, root.rotation);
        targetRotation.RotateAround(pivotPoint.position, transform.right, -angle);

        AdjustToInclineSmoothly();
    }

    void AdjustToInclineSmoothly()
    {
        positionProgress = Vector3.MoveTowards(positionProgress, targetRotation.position, maxDistanceDelta * Time.deltaTime);
        rotationProgress = Quaternion.Euler(rotationProgress.eulerAngles.x, root.eulerAngles.y, root.eulerAngles.z);
        rotationProgress = Quaternion.RotateTowards(rotationProgress, targetRotation.rotation, maxDegreesDelta * Time.deltaTime);
        root.SetPositionAndRotation(positionProgress, rotationProgress);
    }
}
