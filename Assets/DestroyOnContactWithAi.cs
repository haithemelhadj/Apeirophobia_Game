using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnContactWithAi : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
            Debug.Log("touch");

        if(collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("enemy touch");
            Destroy(this.gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
            Debug.Log("touch");
        if(other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("enemy touch");
            Destroy(this.gameObject);
        }
    }
}
