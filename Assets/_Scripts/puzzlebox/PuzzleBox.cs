using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class PuzzleBox : MonoBehaviour
{
    public CinemachineVirtualCamera boxpuzzlecam;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            boxpuzzlecam.Priority = 11;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            boxpuzzlecam.Priority = 9;
        }
    }
}
