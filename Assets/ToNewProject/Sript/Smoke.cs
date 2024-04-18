using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smoke : MonoBehaviour
{
    public void Start()
    {
        Destroy(gameObject, 9f);
    }
    public void Update()
    {
        transform.position += new Vector3(-0.2f, 0, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Player is in smoke");
        }
    }
}
