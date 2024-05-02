using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLoss : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject resumeButton;
    public static void Loss()
    {
        //stop reloading scene and show pause menu instead 

        //.SetActive(true);
        //get option of pause or game over
        //also timescale = 0
        //reload same active scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
