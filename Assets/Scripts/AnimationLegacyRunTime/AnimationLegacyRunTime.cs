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

using UnityEngine;
using System.Collections;

public static class AnimationLegacyRunTime
{
    private static AnimationSetup[] _animations;
    private static Animation _animation;

    public static void Init(int animationSize, Animation animation)
    {
        _animations = new AnimationSetup[animationSize];
        _animation = animation;
    }

    #region CONFIGURATION
    public static void RegisterAnimation( AnimationKeyframe[] keyframes, float sampleTime)
    {
        if(_animations != null && _animations.Length > 0)
        {
            GetNextAnimation().Add(keyframes, sampleTime);
        }
        else
        {
            Debug.LogError("You need to initialize Animation System with the correct number of animations");
        }   
    }

    private static AnimationSetup GetNextAnimation()
    {
        for(int i = 0; i < _animations.Length; i++)
        {
            if(_animations[i] == null)
            {
                _animations[i] = new AnimationSetup();
                return _animations[i];
            }
        }

        Debug.LogError("AnimationLegacyRunTime don't support add new keyframes on runtime");
        return null;
    }
    
    #endregion

    #region START
    public static void Start()
    {
        string targetAnimationToPlay = "";

        for(int i = 0; i < _animations.Length; i++)
        {
            AnimationSetup currentAnimation = _animations[i];
            AnimationClip clip = new AnimationClip();
            clip.legacy = true;

            AnimationCurve curveX = new AnimationCurve();
            AnimationCurve curveY = new AnimationCurve();
            AnimationCurve curveZ = new AnimationCurve();

            for (int k = 0; k < currentAnimation.m_keyframes.Length; k++)
            {
                AnimationKeyframe key = currentAnimation.m_keyframes[k];
                curveX.AddKey(key.m_time, key.m_keyframePosition.x);
                curveY.AddKey(key.m_time, key.m_keyframePosition.y);
                curveZ.AddKey(key.m_time, key.m_keyframePosition.z);
            }

            clip.SetCurve("", typeof(Transform), "localPosition.x", curveX);
            clip.SetCurve("", typeof(Transform), "localPosition.y", curveY);
            clip.SetCurve("", typeof(Transform), "localPosition.z", curveZ);

            if (targetAnimationToPlay == "")
                targetAnimationToPlay = currentAnimation.GetAnimationName();

            _animation.AddClip(clip, currentAnimation.GetAnimationName());

            if(_animations[i].m_animationSpeedMultiply != 0)
            {
                _animation[currentAnimation.GetAnimationName()].speed *= _animations[i].m_animationSpeedMultiply;
            }
            else
            {
                Debug.LogWarning("Your animation multiply is 0!");
            }
        }

        //Play just the first animation
        _animation.Play(targetAnimationToPlay);
    }
    #endregion
}

[System.Serializable]
public class AnimationKeyframe
{
    public Vector3 m_keyframePosition;
    public float m_time;
}

public class AnimationSetup
{
    private string _animationName;
    public AnimationKeyframe[] m_keyframes;
    public float m_animationSpeedMultiply;

    public void Add(AnimationKeyframe[] keyframes, float speedMultiply, string animationName = "AnimationDefault")
    {
        m_keyframes = keyframes;
        m_animationSpeedMultiply = speedMultiply;
        _animationName = animationName;
    }

    public string GetAnimationName()
    {
        return _animationName;
    }
    
}
