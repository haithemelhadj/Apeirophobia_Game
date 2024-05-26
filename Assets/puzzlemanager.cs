using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class puzzlemanager : MonoBehaviour
{
    public Animator anim;
    public TextMeshProUGUI First;
    public TextMeshProUGUI Second;
    public TextMeshProUGUI Third;
    public TextMeshProUGUI Fourth;
    public TextMeshProUGUI Fifth;
    public GameObject UI;
    public AudioSource notification;
    public AudioSource doorOpenSound;
    public void Update()
    {
        if (First.text == "1" && Second.text == "8" && Third.text == "3" && Fourth.text == "6" && Fifth.text == "0")
        {
            Time.timeScale = 1f;
            Debug.Log("Correct");
            Invoke("killUI", 1f);
            Cursor.lockState = CursorLockMode.Locked;
            Invoke("opendoor", 2f);
            
        }
    }
    public void opendoor()
    {
        anim.SetFloat("speed", 1f);
        
    }
    public void killUI()
    {
        UI.SetActive(false);
    }
}
