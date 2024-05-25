using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlaybuttonScript : MonoBehaviour
{
    public AudioSource audioSource;
    
    public void playbutton()
    {
        audioSource.Play();
        SceneManager.LoadScene(1);
        
    }
}
