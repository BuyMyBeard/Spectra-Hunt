using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotBusyAtNormalizedTime : StateMachineBehaviour
{
    [Range(0f, 1f)]
    [SerializeField] float notBusyAt = 1f;

    bool hasWarnedAnimator = false;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        hasWarnedAnimator = false;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!hasWarnedAnimator && stateInfo.normalizedTime >= notBusyAt)
        {
            hasWarnedAnimator = true;
            animator.SetBool("IsBusy", false);
        }
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
