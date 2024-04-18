using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonCam : MonoBehaviour
{
    [Header("Refrences")]
    //public PlayerManager playerManager;
    public Transform orientation;
    public Transform player;
    public Transform playerObj;
    public Rigidbody rb;

    public float rotationSpeed;

        //------------------------------------------------------------------------------------
    //player inputs
    private PLayer_Inputs playerInputs;
    private InputAction move;

    private void OnEnable()
    {
        move = playerInputs.Player.Move; // set player movement inputs 
        playerInputs.Player.Enable();
    }
    private void OnDisable()
    {
        playerInputs.Player.Disable();
    }

        //------------------------------------------------------------------------------------

    private void Update()
    {
        //lock cursor 
        LockCursor();


        //rotate orientation same as camera
        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;

        //if player is hanging he can't rotate
/*if (playerManager.isHanging)
    return;
*/
#if CINEMACHINE_UNITY_INPUTSYSTEM
        //------------------------------------------------------------------------------------
        //alternative player new input system
        Vector3 inputDir2 = orientation.forward * move.ReadValue<Vector2>().y + orientation.right * move.ReadValue<Vector2>().x;
        //------------------------------------------------------------------------------------
#else
        //get player input
        Vector3 inputDir = GetRotationInput();
#endif
        //smoothly change the player object rotation to the input direction
        if (inputDir != Vector3.zero)
        {
            playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
        }
    }

    private Vector3 GetRotationInput()
    {
        float horizontalInput, verticalInput;
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;
        return inputDir;
    }

    
    private void LockCursor()
    {
        bool locked = default;
        if (Input.GetKey(KeyCode.L)) locked = !locked;
        if (locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
