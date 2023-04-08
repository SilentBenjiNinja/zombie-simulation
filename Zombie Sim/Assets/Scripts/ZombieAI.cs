using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAI : MonoBehaviour
{
    float fov = 90;
    float rangeOfVision = 10f;
    float proximityDetectionRange = 5f;
    bool detected;

    private void Update()
    {
        Vector3 zToHooman = hooman.transform.position - transform.position;

        float hoomanAngle = Vector3.Dot(zToHooman.normalized, transform.forward);

        if (zToHooman.magnitude <= proximityDetectionRange)
        {
            detected = true;
        }
        else
        {
            detected = hoomanAngle > Mathf.Cos(.5f * fov * Mathf.Deg2Rad);
            detected &= zToHooman.magnitude <= rangeOfVision;
        }
    }

    #region Debug

    int rays = 5;
    [SerializeField] GameObject hooman;

    private void OnDrawGizmos()
    {
        for (int i = 0; i <= rays; i++)
        {
            float rayAngle = transform.localEulerAngles.y + -.5f * fov + i * (float)(fov / rays);

            float rayX = Mathf.Sin(rayAngle * Mathf.Deg2Rad);
            float rayY = Mathf.Cos(rayAngle * Mathf.Deg2Rad);

            Gizmos.color = detected ? Color.red : Color.cyan;
            Gizmos.DrawRay(transform.position + Vector3.up, new Vector3(rayX, 0, rayY) * rangeOfVision);
        }

        Gizmos.DrawWireSphere(transform.position + Vector3.up, proximityDetectionRange);
    }

    #endregion
}
