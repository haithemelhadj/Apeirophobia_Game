using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    public GameObject PauseMenu;
    public GameObject resumeButton;

    private void Awake()
    {
        playerRef = GameObject.Find("Player").transform;
        checkpoint = playerRef;
        //playerRef=find
    }

    public void GameOver()
    {
        PauseMenu.SetActive(true);
        resumeButton.SetActive(false);
    }

    public void PauseGame()
    {
        PauseMenu.SetActive(true);
        resumeButton.SetActive(true);
    }

    
    //set checkpoints in playerscript
    public Transform playerRef;
    public Transform checkpoint;
    public void RestartFromCheckpoint()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
        playerRef.transform.position = checkpoint.position;
        playerRef.transform.rotation = checkpoint.rotation;
        PauseMenu.SetActive(false);
    }


    //--------------------------done 
    public void Restart()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ResumeGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
        PauseMenu.SetActive(false);
    }
    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
    public void Quit()
    {
        Application.Quit();
    }
}
