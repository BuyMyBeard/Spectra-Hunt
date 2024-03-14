using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OneTimeTrigger : MonoBehaviour
{
    public UnityEvent onTrigger = new();
    bool activatedOnce = false;
    private void OnTriggerEnter(Collider other)
    {
        if (!activatedOnce)
        {
            activatedOnce = true;
            onTrigger.Invoke();
        }
    }
}
