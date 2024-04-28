using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvActivation : MonoBehaviour
{
    public GameObject inventory;

    public void Start()
    {
        Invoke("OpenInventory", 22f);
    }
    public void OpenInventory()
    {
        inventory.SetActive(true);
    }
}
