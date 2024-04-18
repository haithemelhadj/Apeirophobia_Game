using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class TutoCheckpoint : MonoBehaviour
{
    public GameObject barrier;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
           
            Destroy(barrier);
        }
    }
}
