using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class smokespawner : MonoBehaviour
{
    public GameObject smoke;
    public void Start()
    {
       InvokeRepeating("spawnsmoke", 3f,9f);
    }

    public void spawnsmoke()
    {
        Instantiate(smoke, transform.position, Quaternion.identity);
    }
}
