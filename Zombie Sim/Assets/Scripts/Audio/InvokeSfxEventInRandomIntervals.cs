using System.Collections;
using UnityEngine;

public class InvokeSfxEventInRandomIntervals : MonoBehaviour
{
    [SerializeField] SfxEvent sfxEvent;

    [SerializeField] FloatMinMaxValuePair randomIntervalMinMax;

    Coroutine timer;
    bool IsTimerRunning => timer != null;

    void Update()
    {
        if (!IsTimerRunning)
            timer = StartCoroutine(InvokeAfterRandomDelay());

        IEnumerator InvokeAfterRandomDelay()
        {
            yield return new WaitForSeconds(randomIntervalMinMax.RandomValueBetween);

            SfxMessageBus.InvokePlaySfxAtPosition(sfxEvent, transform.position);

            timer = null;
        }
    }
}
