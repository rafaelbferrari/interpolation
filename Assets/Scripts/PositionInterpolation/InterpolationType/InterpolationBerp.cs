using UnityEngine;
using System.Collections;
using System;

public class InterpolationBerp : InterpolationTypeBase
{

    public override IEnumerator InterpolateToPoint(PI_Keyframe keyframe, Transform transform, Action finishCallback)
    {
        if (keyframe.m_delayToStart > 0)
        {
            yield return new WaitForSeconds(keyframe.m_delayToStart);
        }

        float time = 0.0f;
        float duration = keyframe.m_time;

        Vector3 target = keyframe.m_targetPosition != null ? keyframe.m_targetPosition.position : keyframe.m_keyPosition;

        duration *= GetTargetFrameRateMultiply();

        while (time < duration)
        {
            transform.position = Berp(transform.position, target, (time / duration));
            time += Time.deltaTime;
            yield return null;
        }

        if (finishCallback != null) finishCallback();
    }

    private Vector3 Berp(Vector3 v0, Vector3 v1, float t)
    {
        return new Vector3(Berp(v0.x, v1.x, t),
                           Berp(v0.y, v1.y, t),
                           Berp(v0.z, v1.z, t));
    }

    //Mathf calc taken from http://wiki.unity3d.com/index.php?title=Mathfx
    private float Berp(float start, float end, float value)
    {
        value = Mathf.Clamp01(value);
        value = (Mathf.Sin(value * Mathf.PI * (0.2f + 2.5f * value * value * value)) * Mathf.Pow(1f - value, 2.2f) + value) * (1f + (1.2f * (1f - value)));
        return start + (end - start) * value;
    }
}
