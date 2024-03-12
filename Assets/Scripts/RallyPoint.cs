using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RallyPoint : MonoBehaviour
{
    [SerializeField] Mesh arrowMesh;
    [SerializeField] Color color;

    private void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.DrawMesh(arrowMesh, transform.position, transform.rotation);
    }
}
