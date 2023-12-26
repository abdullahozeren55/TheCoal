using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShakeManager : MonoBehaviour
{
    public static CameraShakeManager instance;

    [SerializeField] private float globalShakeForce = 1f;
    [SerializeField] private CinemachineImpulseListener[] listeners;

    private CinemachineImpulseDefinition impulseDefinition;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public void CameraShake(CinemachineImpulseSource impulseSource)
    {
        impulseSource.GenerateImpulseWithForce(globalShakeForce);

    }

    public void ScreenShakeFromProfile(SO_ScreenShakeProfile profile, CinemachineImpulseSource impulseSource)
    {
        //apply settings
        SetUpScreenShakeSettings(profile, impulseSource);

        //screenshake
        impulseSource.GenerateImpulseWithForce(profile.impulseForce);
    }

    private void SetUpScreenShakeSettings(SO_ScreenShakeProfile profile, CinemachineImpulseSource impulseSource)
    {
        impulseDefinition = impulseSource.m_ImpulseDefinition;

        //impulse source settings
        impulseDefinition.m_ImpulseDuration = profile.impulseTime;
        impulseSource.m_DefaultVelocity = profile.defaultVelocity;

        //impulse listener settings
        for (int i = 0; i < listeners.Length; i++)
        {
            listeners[i].m_ReactionSettings.m_AmplitudeGain = profile.listenerAmplitude;
            listeners[i].m_ReactionSettings.m_FrequencyGain = profile.listenerFrequency;
            listeners[i].m_ReactionSettings.m_Duration = profile.listenerDuration;
        }

    }
}
