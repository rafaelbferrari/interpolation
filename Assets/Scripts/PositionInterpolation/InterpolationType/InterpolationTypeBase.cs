using UnityEngine;
using System.Collections;
using System;

public class InterpolationTypeBase
{
    private int _targetFrameRate;

    public virtual IEnumerator InterpolateToPoint(PI_Keyframe keyframe, Transform transform, Action finishCallback)
    {
        yield return null;
    }

    public void SetTargetFrameRate(int frameRate)
    {
        _targetFrameRate = frameRate;
    }

    protected float GetTargetFrameRateMultiply()
    {
        return (float) PositionInterpolation.DEFAULT_FRAME_RATE / _targetFrameRate;
    }
}
