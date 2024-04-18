using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    ////player controller
    //#region Player Manager
    //[Header("Components")]// player components
    //public Rigidbody rb;
    //public Animator animator;
    //public Transform playerObj;

    //#region other functions

    //// mouse lock 
    //private void LockCursor()
    //{
    //    bool locked = default;
    //    if (Input.GetKey(KeyCode.L)) locked = !locked;
    //    if (locked)
    //    {
    //        Cursor.lockState = CursorLockMode.Locked;
    //        Cursor.visible = false;
    //    }
    //    else
    //    {
    //        Cursor.lockState = CursorLockMode.None;
    //        Cursor.visible = true;
    //    }
    //}

    //#region Ground Check

    //// grounded check
    //[SerializeField] LayerMask whatIsGround;
    //[SerializeField] float extraScanDistance;
    //[SerializeField] public float playerHeight;
    //public bool GroundCheck()
    //{
    //    return Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + extraScanDistance, whatIsGround);
    //}

    //#endregion

    //#region Over Head CHeck

    //// over head check
    //public bool OverHeadCheck()
    //{
    //    return Physics.Raycast(transform.position, Vector3.up, playerHeight * 0.5f + extraScanDistance);
    //}

    //#endregion

    //#endregion

    //#region Unity Functions

    ////Gizmos draw distances
    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.blue;
    //    //Debug.Log(GroundCheck());
    //    Gizmos.DrawRay(transform.position, Vector3.up * (playerHeight * 0.5f + extraScanDistance));
    //    Gizmos.DrawRay(transform.position, Vector3.down * (playerHeight * 0.5f + extraScanDistance));
    //}
    //private void Awake()
    //{
    //    playerInputs = new PLayer_Inputs();
    //    CurrMaxMoveSpeed = walkSpeed;
    //    //move = playerInputs.Player.Move;
    //}
    //private void Start()
    //{

    //}
    //private void Update()
    //{
    //    LockCursor();
    //    AnimatorUpdate();
    //    RotatePlayer();
    //    MovementDrag();
    //    HSpeedControl();
    //}
    //private void FixedUpdate()
    //{
    //    MovePlayer();
    //}


    //[Header("Inputs")]//player inputs
    //public float x;
    //public PLayer_Inputs playerInputs;
    //private InputAction move;
    //private void InputEventsSubscriptions()
    //{
    //    playerInputs.Player.Jump.started += DoJump; // when jump button is pressed do jump event
    //    playerInputs.Player.Crouch.started += DoCrouch; // when Crouch button is pressed do Crouch event
    //    playerInputs.Player.Sprint.started += DoSprint; // when Sprint button is pressed do Sprint event
    //}
    //private void InputEventsUnsubscriptions()
    //{
    //    playerInputs.Player.Jump.started -= DoJump; // when jump button is pressed do jump event
    //    playerInputs.Player.Crouch.started -= DoCrouch; // when Crouch button is pressed do Crouch event
    //    playerInputs.Player.Sprint.started -= DoSprint; // when Sprint button is pressed do Sprint event
    //}

    //private void OnEnable()
    //{
    //    InputEventsSubscriptions();
    //    move = playerInputs.Player.Move; // set player movement inputs 
    //    playerInputs.Player.Enable();
    //}

    //private void OnDisable()
    //{
    //    InputEventsUnsubscriptions();
    //    playerInputs.Player.Disable();
    //}

    //#endregion

    //#region animator system
    //private void AnimatorUpdate()
    //{
    //    animator.SetBool("Grounded", GroundCheck());
    //    animator.SetFloat("VelocityH", InputDir().magnitude * CurrMaxMoveSpeed);

    //}


    //#endregion

    //#region inputs manager

    ////player horizental input value
    //public Vector3 InputDir()//return the horizontal input direction based on camera
    //{
    //    if (false)//conditions that prevent the player from moving
    //    {
    //        return Vector3.zero;
    //    }
    //    Vector3 dir = Vector3.zero;

    //    dir += move.ReadValue<Vector2>().x * GetRightDirection();
    //    dir += move.ReadValue<Vector2>().y * GetForwardDirection();
    //    return dir;
    //}



    //#endregion

    //#region camera rotaion
    //[SerializeField] Camera playerCamera;

    ////get camera direction
    //private Vector3 GetRightDirection()//return the vector 3 of the camera right 
    //{
    //    Vector3 right = playerCamera.transform.right;
    //    right.y = 0;
    //    return right.normalized;
    //}
    //private Vector3 GetForwardDirection()//return the vector 3 of the camera forward
    //{
    //    Vector3 forward = playerCamera.transform.forward;
    //    forward.y = 0;
    //    return forward.normalized;
    //}

    //#endregion
    ////set camera mods
    //#region camera states
    ////camera state
    //[Header("Camera State")]
    //private Cstate cameraState;
    //private enum Cstate
    //{
    //    normal,
    //    sprinting,
    //    crouching,
    //    hanging
    //}

    ///*
    //// save last state to et back to
    //private Pstate lastState;
    //private Pstate LastState(Pstate state)
    //{
    //    state = playerState;
    //    return state;
    //}
    //*/

    //#endregion

    //#endregion

}
