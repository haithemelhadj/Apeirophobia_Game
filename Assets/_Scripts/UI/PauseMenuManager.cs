using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    public GameObject PauseMenu;
    public GameObject resumeButton;


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
    public GameObject playerRef;
    public Transform checkpoint;
    /*
    private void Awake()
    {
        playerRef = GameObject.Find("Player").transform;
        checkpoint = playerRef;
        //playerRef=find
    }
    */


    public GameObject Boss;
    public Transform BossStartPos;
    public void RestartFromCheckpoint()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
        //Debug.Log("before :" + playerRef.transform.position);
        playerRef.transform.position = checkpoint.position;
        playerRef.transform.rotation = checkpoint.rotation;
        //Debug.Log("after :"+playerRef.transform.position);
        PauseMenu.SetActive(false);
        PlayerMovementTest1.gameOver = false;
        if (Boss.GetComponent<AiAgentTry3>().isInLevelThree)
        {
            Boss.transform.position = BossStartPos.position;
            Boss.transform.rotation = BossStartPos.rotation;
        }

        //Debug.Log("after 2:"+playerRef.transform.position);
    }

    private void LateUpdate()
    {
        //Debug.Log("after after:"+playerRef.transform.position);

    }
    //--------------------------done 
    public void Restart()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
        PlayerMovementTest1.gameOver = false;
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
