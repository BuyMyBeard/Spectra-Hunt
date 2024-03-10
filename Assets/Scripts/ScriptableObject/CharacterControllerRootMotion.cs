using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerRootMotion : StateMachineBehaviour
{
    CharacterController characterController;
    private void Awake()
    {
        characterController = FindObjectOfType<CharacterController>();
    }
    override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log($"Moved {animator.deltaPosition}");
        characterController.Move(animator.deltaPosition /* + Vector3.up * foxMovement.Gravity*/);
        // characterController.transform.rotation *= animator.deltaRotation;
    }
}
