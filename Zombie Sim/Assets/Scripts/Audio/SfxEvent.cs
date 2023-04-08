using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName = "Create SfxEvent...",
    fileName = "SE_", order = 0)]
public class SfxEvent : ScriptableObject
{
    [SerializeField]
    private List<AudioClip> audioClipPool;

    [SerializeField]
    public bool loop;

    [Header("Variations")]

    [SerializeField, Range(0, 1)]
    private float baseVolume = 1;

    [SerializeField]
    private FloatMinMaxValuePair volumeVariation;

    [SerializeField, Range(-3, 3)]
    private float basePitch = 1;

    [SerializeField]
    private FloatMinMaxValuePair pitchVariation;

    public AudioClip RandomClipFromPool =>
        audioClipPool[Random.Range(0, audioClipPool.Count)];

    public float RandomVolumeVariation =>
        Mathf.Clamp01(baseVolume + volumeVariation.RandomValueBetween);

    public float RandomPitchVariation =>
        Mathf.Clamp(basePitch + pitchVariation.RandomValueBetween, -3, 3);

    [Header("Priority")]

    [SerializeField, Range(0, 256)]
    public int priority = 0;

    [SerializeField, Range(1, 5)]
    public int maxPlayingSimultaneously = 3;

    [SerializeField, Range(0, 10f)]
    public float cooldown;
}
