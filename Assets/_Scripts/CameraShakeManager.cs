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

    public void ScreenShakeFromWeapon(WeaponScreenShakeDetails screenShakeDetails, CinemachineImpulseSource impulseSource)
    {
        SetUpScreenShakeSettings(screenShakeDetails, impulseSource);      
        impulseSource.GenerateImpulseWithForce(screenShakeDetails.impulseForce);
    }

    private void SetUpScreenShakeSettings(WeaponScreenShakeDetails screenShakeDetails, CinemachineImpulseSource impulseSource)
    {
        impulseDefinition = impulseSource.m_ImpulseDefinition;

        //impulse source settings
        impulseDefinition.m_ImpulseDuration = screenShakeDetails.impulseTime;
        impulseSource.m_DefaultVelocity = screenShakeDetails.defaultVelocity;

        //impulse listener settings
        for (int i = 0; i < listeners.Length; i++)
        {

            listeners[i].m_ReactionSettings.m_AmplitudeGain = screenShakeDetails.listenerAmplitude;
            listeners[i].m_ReactionSettings.m_FrequencyGain = screenShakeDetails.listenerFrequency;
            listeners[i].m_ReactionSettings.m_Duration = screenShakeDetails.listenerDuration;

        }

    }
}
