using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTopDownCamera : MonoBehaviour
{
    //public GameObject topDownCamera;
    public CinemachineVirtualCamera topDownCinemachineCamera;



    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            Debug.Log("switch to top down");
            topDownCinemachineCamera.Priority = 12;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("switch to normal");
            topDownCinemachineCamera.Priority = 8; ;
        }
    }
}
