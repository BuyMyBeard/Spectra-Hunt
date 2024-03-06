using System.Collections.Generic;
using UnityEngine;

public class IdleBehaviour : StateMachineBehaviour
{
    [SerializeField] float actionMinCooldown = 2;
    [SerializeField] float actionMaxCooldown = 10;
    [SerializeField] List<WeightedAction<string>> idleActions;
    float idleActionTimer;
    bool waitingToFinish = false;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ResetTimer();
        animator.SetBool("IsIdle", true);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (waitingToFinish)
        {
            bool isFinished = animator.GetBool("IdleActionIsFinished");
            if (isFinished)
            {
                ResetTimer();
                waitingToFinish = false;
                animator.SetBool("IdleActionIsFinished", false);
            }
        }
        else
        {
            idleActionTimer -= Time.deltaTime;
            if (idleActionTimer <= 0)
            {
                animator.SetTrigger(idleActions.PickRandom());
                waitingToFinish = true;
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("IsIdle", false);
    }
    private void ResetTimer() => idleActionTimer = Random.Range(2, 10);
}
