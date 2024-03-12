using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HareManager : MonoBehaviour
{
    [SerializeField] Patrol mainHare;
    [SerializeField] Patrol[] coloredHares;
    // Start is called before the first frame update
    void Start()
    {
        foreach (var hare in coloredHares)
        {
            hare.GetComponent<NavMeshAgent>().enabled = false;
            hare.GetComponent<Animator>().SetBool("IsWalking", true);
            hare.GetComponent<Hare>().enabled = false;
        }
        mainHare.StartPatrolling();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        foreach(var hare in coloredHares)
        {
            hare.transform.SetPositionAndRotation(mainHare.transform.position, mainHare.transform.rotation);
        }
    }
}
