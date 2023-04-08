using System;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public struct FloatMinMaxValuePair
{
    [SerializeField] public float min;
    [SerializeField] public float max;

    public float RandomValueBetween => Random.Range(min, max);
}
