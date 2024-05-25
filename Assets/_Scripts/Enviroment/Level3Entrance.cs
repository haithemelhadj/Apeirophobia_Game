using Cinemachine;
using UnityEngine;

public class Level3Entrance : MonoBehaviour
{
    public GameObject aiAgent;
    public AiAgentTry3 aiScript;
    public GameObject playerRef;
    public Transform[] doorEntrance;
    public float waitTime;
    public CinemachineVirtualCamera lvl3Cam;
    public GameObject inventory;
    private PlayerMovementTest1 playerScript;
    private void Start()
    {
        //AiScript = AiAgent.GetComponent<AiAgentTry3>();
        playerScript = playerRef.GetComponent<PlayerMovementTest1>();

        ManageAi();

        //manage ui
        inventory.SetActive(false);
        //manage player 
        playerScript.SprintSpeed = 5.4f;
        //playerScript.walkSpeed = playerScript.SprintSpeed;
        //manage enviroment
        //start lvl 3 camera
        lvl3Cam.Priority = 20;
        playerScript.walkSpeed = 0;
        playerScript.CurrMaxMoveSpeed = 0;
        playerScript.rotationSpeed = 0;
        lvl3Cam.LookAt = aiAgent.transform;
        lvl3Cam.m_Lens.FieldOfView = 50;
    }

    /*
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            ManageAi();

            //manage ui
            inventory.SetActive(false);
            //manage player 
            playerScript.SprintSpeed = 7;
            playerScript.walkSpeed = playerScript.SprintSpeed;
            //manage enviroment
            //start lvl 3 camera
            lvl3Cam.Priority = 20;

        }
    }
    /*
    */

    private void ManageAi()
    {
        //ai 

        //teleport ai to the door with animation
        aiAgent.transform.position = doorEntrance[0].position;
        aiScript.walkPoint = doorEntrance[0].position;
        aiScript.walkPointSet = true;
        aiScript.currentPoint = 0;
        //AiScript.navAgent.SetDestination(DoorEntrance[0].position);
        // make a new patrol path for the ai
        aiScript.points = null;
        aiScript.points = doorEntrance;
        aiScript.waitTime = 0;
        aiAgent.GetComponent<MeshRenderer>().enabled = false;
        Invoke("startLvl3", waitTime);
    }

    public void startLvl3()
    {
        // start level 3 lofic in ai
        aiScript.isInLevelThree = true;
        playerScript.walkSpeed = playerScript.SprintSpeed;
        playerScript.CurrMaxMoveSpeed = playerScript.walkSpeed;
        playerScript.rotationSpeed = 9f;
        lvl3Cam.LookAt = playerRef.transform;
        lvl3Cam.m_Lens.FieldOfView = 70;
    }
}
