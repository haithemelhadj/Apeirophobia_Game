using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class crouchUI : MonoBehaviour
{
    public GameObject KillUI;
    public GameObject OpenUI;
    public AudioSource notification;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            notification.Play();
            KillUI.SetActive(false);
            OpenUI.SetActive(true);

        }
        
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
            Destroy(gameObject);
    }
}
