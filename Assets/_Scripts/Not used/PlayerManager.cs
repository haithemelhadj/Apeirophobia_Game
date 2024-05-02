using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header("other scripts")]
    public PlayerMovement playerMovement;
    public ThirdPersonCam thirdPersonCam;
    public PlayerJump playerJump;

    [Header("Components")]
    //public PlayerManager playerManager;
    public Rigidbody rb;
    //public Animator animator;

    [Header("Bools")]
    public bool isHanging;
    public bool grounded;


    [Header("Keybinds")]
    public KeyCode sprint = KeyCode.LeftShift;
    public KeyCode crouch = KeyCode.LeftControl;
    public KeyCode jumpKey = KeyCode.Space;
    


    [Header("Ground Check")]
    public bool isUnder;
    [Range(0f, 1f)] public float extraScanDistance = 0.05f;
    //[Range(0f, 1f)] public float extraScanDistance = 0.05f;
    public float playerHeight = 2;
    public LayerMask whatIsGround;



    private void Update()
    {
        //head check
        isUnder = Physics.Raycast(transform.position, Vector3.up, playerHeight * 0.5f + extraScanDistance);

        //ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + extraScanDistance, whatIsGround);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        //draw ground check ray
        Gizmos.DrawRay(transform.position, Vector3.down * (playerHeight * 0.5f + extraScanDistance));
        // draw head check ray
        Gizmos.DrawRay(transform.position, Vector3.up * extraScanDistance);
        //draw edge grab check ray
        Gizmos.DrawRay(playerJump.LineDownStart, Vector3.Distance(playerJump.LineDownStart, playerJump.LineDownEnd) * Vector3.down);

    }
}
