using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxAttack : MonoBehaviour
{
    FoxAttackHitbox hitbox;
    private void Awake()
    {
        hitbox = GetComponentInChildren<FoxAttackHitbox>();
    }
    public void OnEnableHitbox()
    {
        hitbox.ColliderEnabled = true;
    }
    public void OnDisableHitbox()
    {
        hitbox.ColliderEnabled = false;
    }
}
