using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level3Entrance : MonoBehaviour
{
    public GameObject AiAgent;
    public AiAgentTry3 AiScript;
    public GameObject Player;
    public Transform[] DoorEntrance;
    public float waitime;
    public CinemachineVirtualCamera lvl3Cam;


    private void Start()
    {
        //AiScript = AiAgent.GetComponent<AiAgentTry3>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) 
        {
            Debug.Log("player touch");
            //teleport ai to the door with animation
            AiAgent.transform.position = DoorEntrance[0].position;
            // make a new patrol path for the ai
            AiScript.points = null;
            AiScript.points = DoorEntrance;
            AiScript.waitTime = 0;
            //start lvl 3 camera
            lvl3Cam.Priority = 20;
            
            Invoke("startLvl3", waitime);
        }
    }

    public void startLvl3()
    {
        // start level 3 lofic in ai
        AiScript.isInLevelThree = true;

    }
}
