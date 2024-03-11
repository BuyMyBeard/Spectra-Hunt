using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class FoxAttackHitbox : MonoBehaviour
{
    new Collider collider;
    readonly List<Hare> haresHit = new();
    public bool ColliderEnabled
    {
        get => collider.enabled;
        set
        {
            collider.enabled = value;
            haresHit.Clear();
        }
    }
    void Awake()
    {
        collider = GetComponent<Collider>();
        ColliderEnabled = false;
        collider.isTrigger = true;
    }
    private void Start()
    {
        GetComponent<Rigidbody>().isKinematic = true;
    }
    void OnTriggerEnter(Collider other)
    {
        Hare hare = other.GetComponentInParent<Hare>();
        if (hare == null)
            return;
        // To avoid hitting the same enemy multiple times with the same attack
        else if (!haresHit.Contains(hare))
        {
            haresHit.Add(hare);
            hare.ReceiveDamage();
        }
    }
}
