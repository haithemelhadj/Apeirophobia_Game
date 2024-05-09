using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointScript : MonoBehaviour
{
    public bool isInLvlOne;
    public PauseMenuManager pauseMenuManager;
    private void Awake()
    {
        //pauseMenuManager = GameObject.Find("PauseMenuManager").GetComponent<PauseMenuManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && isInLvlOne)
        {
            pauseMenuManager.checkpoint = transform;
        }
    }
}
