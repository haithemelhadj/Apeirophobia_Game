using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class closepaper : MonoBehaviour
{
    public GameObject paper;


    public void close()
    {
        paper.SetActive(false);
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
    }
}
