using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;


public class MenuButtons : MonoBehaviour
{

    public void Quit()
    {
        //quit
        Application.Quit();
    }

    public void settingsScene()
    {
        //load settings scene
        SceneManager.LoadScene("settings");
    }

    public void backButton()
    {
        SceneManager.LoadScene(0);
    }

    //public GameObject[] buttons;
    void Start()
    {
       /* for(var i = 0; i < buttons.Length; i++)
        {
            {
                buttons[i].transform.DOLocalMoveX(300f +i * 150, 1f).SetEase(Ease.OutExpo).SetDelay(i * 0.1f);
            }
        }
        */
    }

    // Update is called once per frame
    void Update()
    {
        
    }
  /*  public void disableButtons()
    {
        for (var i = 0; i < buttons.Length; i++)
        {
            {
                buttons[i].transform.DOLocalMoveX(-3000f + i * 150, 1f).SetEase(Ease.OutExpo).SetDelay(i * 0.1f);
            }
        }
    }*/
}
