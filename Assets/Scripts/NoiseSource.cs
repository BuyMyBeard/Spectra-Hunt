using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum NoiseLevel { Quiet, Weak, Strong }
public interface INoiseSource
{
    public UnityEvent<Vector3, NoiseLevel> EmitNoise { get; set; }
}
