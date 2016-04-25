using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animation))]
public class LegacyDemonstration : MonoBehaviour
{
    
    [SerializeField]
    public KeyframeSetup[] m_animationKeyframes;
    
    void Start()
    {
        AnimationLegacyRunTime.Init(m_animationKeyframes.Length, GetComponent<Animation>());
        for(int i = 0; i < m_animationKeyframes.Length; i++)
        {
            AnimationLegacyRunTime.RegisterAnimation(m_animationKeyframes[i].m_animationPositions, m_animationKeyframes[i].m_animationSpeedMultiply);
        }
        AnimationLegacyRunTime.Start();
    }
}

[System.Serializable]
public class KeyframeSetup
{
    [Header("Add positional keyframes for an animation")]
    public AnimationKeyframe[] m_animationPositions;

    [Header("Multiply speed animation")]
    public float m_animationSpeedMultiply = 1f;
}

