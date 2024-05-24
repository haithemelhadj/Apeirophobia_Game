using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideUIScript : MonoBehaviour
{
    public GameObject UI;

    public void OnTriggerEnter(Collider other)
    {
       if(other.gameObject.tag == "Player")
        {
            UI.SetActive(true);
        }
    }
    public  void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            UI.SetActive(false);
            GameObject.Destroy(gameObject);
        }
    }

}
