using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wasdUI : MonoBehaviour
{
    public GameObject UI;
    public GameObject secondUI;
    public AudioSource notification;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            UI.SetActive(true);
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            notification.Play();
            UI.SetActive(false);
            GameObject.Destroy(gameObject);
            secondUI.SetActive(true);
        }
    }
}
