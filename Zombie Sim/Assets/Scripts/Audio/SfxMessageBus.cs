using System;
using UnityEngine;

public static class SfxMessageBus
{
    public static event Action<SfxEvent, Vector3> PlaySfxAtPosition;
    public static void InvokePlaySfxAtPosition(SfxEvent sfxEvent, Vector3 position) =>
        PlaySfxAtPosition?.Invoke(sfxEvent, position);
}
