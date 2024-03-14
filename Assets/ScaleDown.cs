using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleDown : MonoBehaviour
{
    public float scaleDownSpeed = -1f;

    // Update is called once per frame
    void Update()
    {
        transform.localScale = transform.localScale + new Vector3(scaleDownSpeed, scaleDownSpeed, scaleDownSpeed) * Time.deltaTime;
        if (transform.localScale.x < 0f)
        {
            Destroy(gameObject);
        }
    }
}
