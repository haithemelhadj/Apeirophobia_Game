using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseUI : MonoBehaviour
{
    public GameObject UI;
    public void Close()
    {
        UI.SetActive(false);
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
    }
}
