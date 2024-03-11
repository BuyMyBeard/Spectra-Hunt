using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class HideInBushes : MonoBehaviour
{
    [SerializeField] Transform bushContainer;
    [SerializeField] float minDistance = 10;
    [SerializeField] float minDistance2 = 10;
    readonly List<Transform> bushes = new();
    Transform currentBush;
    NavMeshAgent agent;
    // Start is called before the first frame update
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        foreach(Transform t in bushContainer.transform)
        {
            if (t.name == "Bush_01" || t.name == "Bush_02")
                bushes.Add(t);
        }
    }

    Vector3 SeekNewBush()
    {
        bushes.Shuffle();

        Transform found = null;
        if (currentBush != null)
            found = bushes.Find((bush) => Vector3.Distance(currentBush.position, bush.position) > minDistance);

        if (currentBush != null && found == null)
            found = bushes.Find((bush) => Vector3.Distance(currentBush.position, bush.position) > minDistance2);

        if (found == null)
        {
            found = bushes.First();
        }
        Debug.Log($"Distance : {Vector3.Distance(currentBush != null ? currentBush.position : transform.position, found.position)}");
        currentBush = found;
        return currentBush.position;
    }

    [ContextMenu("Go to new bush")]
    public void GoToNewBush()
    {
        agent.SetDestination(SeekNewBush());
    }
}
