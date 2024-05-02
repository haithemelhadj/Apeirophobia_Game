using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddLevel3CamShake : MonoBehaviour
{
    public float intensity = 1.0f;
    public CinemachineVirtualCamera cinemachineCamera;
    public bool isInLevel3;
    private void Awake()
    {
        if(isInLevel3)
        {
            cinemachineCamera = GetComponent<CinemachineVirtualCamera>();
        }
    }
    public void shakeCam(float intensity, float time)
    {
        if(isInLevel3)
        cinemachineCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = intensity;
    }
}
