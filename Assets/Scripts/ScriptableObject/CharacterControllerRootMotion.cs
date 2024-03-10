using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerRootMotion : StateMachineBehaviour
{
    CharacterController characterController;
    FoxMovement foxMovement;
    private void Awake()
    {
        characterController = FindObjectOfType<CharacterController>();
        foxMovement = FindObjectOfType<FoxMovement>();
    }
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        foxMovement.RootOrigin = foxMovement.rootBone.localPosition;
        foxMovement.PrevRootPosition = foxMovement.RootOrigin;
    }
    override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        foxMovement.RootDeltaMovement = foxMovement.rootBone.localPosition - foxMovement.PrevRootPosition;
        foxMovement.rootBone.localPosition = foxMovement.RootOrigin;
        Vector3 worldDelta = foxMovement.transform.TransformVector(foxMovement.RootDeltaMovement) / foxMovement.transform.lossyScale.x;
        characterController.Move(worldDelta + Vector3.up * foxMovement.Gravity);
        // characterController.transform.rotation *= animator.deltaRotation;
    }
}
