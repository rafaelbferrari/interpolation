using UnityEngine;
using System.Collections;
using System;

public class InterpolationOut : InterpolationTypeBase
{

    public override IEnumerator InterpolateToPoint(PI_Keyframe keyframe, Transform transform, Action finishCallback)
    {
        if(keyframe.m_delayToStart > 0)
        {
            yield return new WaitForSeconds(keyframe.m_delayToStart);
        }
        float time = 0.0f;
        float duration = keyframe.m_time;

        Vector3 target = keyframe.m_targetPosition != null ? keyframe.m_targetPosition.position : keyframe.m_keyPosition;

        duration *= GetTargetFrameRateMultiply();

        while (time < duration)
        {
            transform.position = Linear(transform.position, target, (time / duration));
            time += Time.deltaTime;
            yield return null;
        }

        if (finishCallback != null) finishCallback();
    }

    private Vector3 Linear(Vector3 v0, Vector3 v1, float t)
    {
        return new Vector3(Linear(v0.x, v1.x, t),
                           Linear(v0.y, v1.y, t),
                           Linear(v0.z, v1.z, t));
    }

    private float Linear(float v0, float v1, float t)
    {
        return (1 - t) * v0 + t * v1;
    }
}
