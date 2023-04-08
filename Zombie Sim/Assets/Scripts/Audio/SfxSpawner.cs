using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SfxSpawner : MonoBehaviour
{
    [SerializeField, Range(1, 64)]
    private int startAmount = 16;

    [SerializeField]
    private AudioSource audioSourcePrefab;

    [SerializeField]
    private Transform audioSourceParent;

    private ObjectPool<AudioSource> audioSourcePool;

    // TODO: possibly hash these?
    private List<int> currentlyPlayingSfxEvents = new List<int>();
    private List<int> sfxEventsOnCooldown = new List<int>();

    private void Awake()
    {
        audioSourcePool = new ObjectPool<AudioSource>(audioSourcePrefab,
            audioSourceParent, startAmount, "PooledAudioSource");
    }

    private void OnEnable() => SfxMessageBus.PlaySfxAtPosition += OnPlaySfxAtPosition;
    private void OnDisable() => SfxMessageBus.PlaySfxAtPosition -= OnPlaySfxAtPosition;

    private void OnPlaySfxAtPosition(SfxEvent sfxEvent, Vector3 position)
    {
        int sfxEventHashed = sfxEvent.GetHashCode();

        if (sfxEventsOnCooldown.Contains(sfxEventHashed) ||
            currentlyPlayingSfxEvents.Where(x => x == sfxEventHashed)
                .ToList().Count >= sfxEvent.maxPlayingSimultaneously)
            return;

        AudioSource audioSource = audioSourcePool.Next();
        audioSource.transform.position = position;

        SetAudioSourceParameters(audioSource, sfxEvent);

        if (sfxEvent.cooldown > 0)
        {
            sfxEventsOnCooldown.Add(sfxEventHashed);

            InvokeActionDelayed(sfxEvent.cooldown, () =>
                sfxEventsOnCooldown.Remove(sfxEventHashed));
        }

        audioSource.gameObject.SetActive(true);
        audioSource.Play();

        currentlyPlayingSfxEvents.Add(sfxEventHashed);

        if (!sfxEvent.loop)
            InvokeActionDelayed(audioSource.clip.length, () =>
            {
                audioSource.Stop();
                audioSource.gameObject.SetActive(false);

                currentlyPlayingSfxEvents.Remove(sfxEventHashed);
            });
    }

    // TODO: make tool somewhere else
    void InvokeActionDelayed(float delay, Action action)
    {
        StartCoroutine(InvokeDelayed());

        IEnumerator InvokeDelayed()
        {
            yield return new WaitForSeconds(delay);

            action?.Invoke();
        }
    }

    void SetAudioSourceParameters(AudioSource audioSource, SfxEvent sfxEvent)
    {
        audioSource.clip = sfxEvent.RandomClipFromPool;
        audioSource.loop = sfxEvent.loop;
        audioSource.priority = sfxEvent.priority;
        audioSource.volume = sfxEvent.RandomVolumeVariation;
        audioSource.pitch = sfxEvent.RandomPitchVariation;
    }
}
