using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Target : MonoBehaviour
{
    public LayerMask playerMask;
    public float length;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f,0.5f,0));
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction * length , Color.red);
        if (Physics.Raycast(ray, out hit, length, ~playerMask))
        {
            Debug.Log(hit.transform.name);
        }
    }
}
