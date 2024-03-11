using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RandomSoundDef", menuName = "Scriptables/RandomSoundDef")]
public class RandomSoundDef : ScriptableObject
{
    public WeightedAction<AudioClip>[] soundPool;
    public float volume = 1;
    public bool affectedByNoiseLevel = true;
}
