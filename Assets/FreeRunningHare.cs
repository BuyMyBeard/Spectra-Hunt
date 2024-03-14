using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FreeRunningHare : MonoBehaviour
{
    Animator animator;
    NavMeshAgent agent;
    Transform fox;
    [SerializeField] float runSpeed = 3;
    bool runningAway = false;
    [SerializeField] float turnSpeed = 300;
    [SerializeField] float runAwayDistance = 10;
    [SerializeField] float stopRunningAwayDistance = 25;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        fox = FindObjectOfType<FoxMovement>(true).transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (!runningAway && Vector3.Distance(fox.position, transform.position) < runAwayDistance)
           runningAway = true;
        else if (runningAway && Vector3.Distance(fox.position, transform.position) > stopRunningAwayDistance)
            runningAway = false;
        animator.SetBool("IsRunning", runningAway);
        if (runningAway)
        {
            Vector3 opposite = transform.position - fox.position;
            opposite.y = 0;
            opposite = opposite.normalized;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(opposite), turnSpeed * Time.deltaTime);
            agent.Move(runSpeed * Time.deltaTime * transform.forward);
        }
    }
}
