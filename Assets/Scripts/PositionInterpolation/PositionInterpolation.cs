//The MIT License(MIT)

//Copyright(c) 2016 Rafael Ferrari

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.

///<summary>
///summary description
///</summary>
/// 
using UnityEngine;
using System.Collections;

public class PositionInterpolation : MonoBehaviour
{
    public const int DEFAULT_FRAME_RATE = 60;

    [SerializeField]
    private PI_Keyframe[] _keyframes;
    private InterpolationTypeBase _currentInterpolation;
    private int _currentFrame = -1;

    private InterpolationLinear _interpolationLinear;
    private InterpolationOut _interpolationOut;
    private InterpolationBerp _interpolationBerp;
    
    public int m_targetFrameRate = DEFAULT_FRAME_RATE;

    [System.Serializable]
    public enum InterpolationType
    {
        LINEAR = 1,
        OUT,
        BERP,
        COUNT
    }

    public void Start()
    {
        Init();
    }

    #region INITIALIZE
    private void Init()
    {
        InitializeTweens();
        GoToNextFrame();
        if(m_targetFrameRate <= 0)
        {
            m_targetFrameRate = DEFAULT_FRAME_RATE;
            Debug.LogError("Your target framerate is 0 or less!");
        }
    }

    private void InitializeTweens()
    {
        _interpolationLinear = new InterpolationLinear();
        _interpolationOut = new InterpolationOut();
        _interpolationBerp = new InterpolationBerp();
    }
    #endregion

    public void AddKeyframes(PI_Keyframe[] keyframes)
    {
        _keyframes = keyframes;
    }
    
    #region ANIMATION CONTROLL
    private void GoToNextFrame()
    {
        _currentFrame++;
        if(_currentFrame >= _keyframes.Length )
        {
            _currentFrame = 0;

        }

        PI_Keyframe currentKeyframe = _keyframes[_currentFrame];

        GetCurrentInterpolation(currentKeyframe).SetTargetFrameRate(m_targetFrameRate);

        StartCoroutine(GetCurrentInterpolation(currentKeyframe).InterpolateToPoint(currentKeyframe,transform,GoToNextFrame));

    }

    private InterpolationTypeBase GetCurrentInterpolation(PI_Keyframe keyframe)
    {
        switch(keyframe.m_interpolationType)
        {
            case InterpolationType.LINEAR:
                return _interpolationLinear;
            case InterpolationType.OUT:
                return _interpolationOut;
            case InterpolationType.BERP:
                return _interpolationBerp;
            default:
                return _interpolationLinear;

        }
    }
    #endregion

}

/***
Using PI_ on the begin of the classes to differentiate class name from Legacy Demonstrations
***/
[System.Serializable]
public class PI_Keyframe
{

    [Header("If you don't add target position, vector values will be used.")]
    public Transform m_targetPosition;
    public Vector3 m_keyPosition;
    
    [Header("Interpolation setup")]
    public float m_delayToStart;
    public float m_time;
    [SerializeField]
    public PositionInterpolation.InterpolationType m_interpolationType;
}
