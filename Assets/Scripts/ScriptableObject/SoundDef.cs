using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundDef", menuName = "Scriptables/SoundDef")]
public class SoundDef : ScriptableObject
{
    public AudioClip clip;
    public float volume = 1;
    public bool affectedByNoiseLevel = true;
}
