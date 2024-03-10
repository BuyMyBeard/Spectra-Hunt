using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VFXFastForward : MonoBehaviour
{
    void Start()
    {
        foreach(var vfx in GetComponentsInChildren<VisualEffect>())
        {
            vfx.Simulate(15, 200);
        }
    }
}
