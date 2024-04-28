using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class Playbutton : MonoBehaviour
{
    public PlayableDirector timeline;
    public float TimeLineLength;
    public MenuButtons canva;

    void Start()
    {
        
    }

    
    public void PlayTimeline()
    {
        timeline.Play();
    }
    public void gotoScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Aziz Scene");
    }
    public void PlayPressed()
    {
        //canva.disableButtons();
        Invoke("PlayTimeline", 1f);
        Invoke("gotoScene", TimeLineLength +1f);
        
    }
}
