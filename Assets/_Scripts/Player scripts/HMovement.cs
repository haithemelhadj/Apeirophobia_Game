using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HMovement : MonoBehaviour
{
    ////other scripts
    //public PlayerController playerController;
    //// Hmovement
    //#region player movement System

    //#region player rotation handler

    ////rotate player based on camera direction
    //[SerializeField] float rotationSpeed = 7f;
    //private void RotatePlayer()
    //{
    //    Vector3 inputDir = playerController.InputDir();
    //    //smoothly change the player object rotation to the input direction
    //    if (inputDir != Vector3.zero)
    //    {
    //        playerController.playerObj.forward = Vector3.Slerp(playerController.playerObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
    //    }
    //}

    //#endregion

    //#region movement handler
    //[Header("Movement Speed")]//player movement variables
    //// horizontal movement
    //[SerializeField] float CurrMaxMoveSpeed;
    //[SerializeField] float acceleration = 2;
    //[SerializeField] float deceleration = 3;
    //[SerializeField] float walkSpeed = 5;

    //private void MovePlayer()
    //{
    //    Vector3 inputDir = playerController.InputDir();
    //    if (inputDir != Vector3.zero)// will add more conditions soon
    //    {
    //        Vector3 currentSpeed = Vector3.MoveTowards(playerController.rb.velocity, inputDir.normalized * CurrMaxMoveSpeed, acceleration);
    //        playerController.rb.velocity = new Vector3(currentSpeed.x, playerController.rb.velocity.y, currentSpeed.z);
    //        //animator.SetFloat("VelocityH",
    //    }
    //    else
    //    {
    //        playerController.rb.velocity = Vector3.MoveTowards(playerController.rb.velocity, Vector3.zero, deceleration);
    //    }
    //}

    //// movement drag
    ////float playerDrag;
    //[SerializeField] float groundDrag = 3.5f;
    //[SerializeField] float AirDrag = 2;
    //private void MovementDrag()
    //{
    //    if (playerController.GroundCheck())
    //    {
    //        //playerDrag = groundDrag;
    //        playerController.rb.drag = groundDrag;
    //    }
    //    else
    //    {
    //        //playerDrag = AirDrag;
    //        playerController.rb.drag = AirDrag;
    //    }
    //}

    ////speed control
    ////float maxMoveSpeed;
    //private void HSpeedControl()
    //{
    //    Vector3 flatVel = new Vector3(playerController.rb.velocity.x, 0f, playerController.rb.velocity.z);
    //    //limit  velocity if needed
    //    if (flatVel.magnitude > CurrMaxMoveSpeed)
    //    {
    //        Vector3 limitedVel = flatVel.normalized * CurrMaxMoveSpeed;
    //        playerController.rb.velocity = new Vector3(limitedVel.x, playerController.rb.velocity.y, limitedVel.z);
    //    }
    //}

    //#endregion


    //// player state exists to check if the player is crouching or sprinting or walking, i don't like it, i think i'll go to a bool based system
    //#region player states
    ////player state
    //[Header("Player State")]
    //private Pstate playerState;
    //private enum Pstate
    //{
    //    //states
    //    standing,
    //    crouching,
    //    hanging,
    //    jumping,//???

    //    moving,//velocity float
    //    idle,//velocity float


    //    walking,
    //    sprinting,
    //    crouched_walking
    //}
    //#endregion


    //#region crouch handler
    //[SerializeField] float CrouchSpeed = 3f;
    //public CapsuleCollider collider;
    //private void DoCrouch(InputAction.CallbackContext context)
    //{
    //    if (!playerController.GroundCheck())
    //        return;

    //    //crouch state
    //    if (playerState == Pstate.crouching && !playerController.OverHeadCheck())
    //    {
    //        playerState = Pstate.walking;
    //        CurrMaxMoveSpeed = walkSpeed;
    //        playerController.animator.SetTrigger("Stand");
    //        //change collider size
    //        collider.height = playerController.playerHeight;//1
    //        collider.center = new Vector3(0f, 0.5f, 0f);// 0,0.5,0 (+ radius = 0.2)

    //    }
    //    else
    //    {
    //        playerState = Pstate.crouching;
    //        CurrMaxMoveSpeed = CrouchSpeed;
    //        playerController.animator.SetTrigger("Crouch");
    //        Debug.Log("crouch");
    //        //change collider size
    //        collider.height = playerController.playerHeight * 0.6f;//0.6
    //        collider.center = new Vector3(0f, 0.3f, 0f);// 0,0.3,0 (+ radius = 0.2)

    //    }
    //    /*

    //private void Crouching()
    //{
    //    if (Input.GetKeyDown(playerManager.crouch) && playerManager.grounded)
    //    {
    //        if (isCrouching && !playerManager.isUnder)
    //        {
    //            moveSpeed = walkSpeed;
    //            isCrouching = false;
    //            //change collider size
    //            playerCollider.height = playerManager.playerHeight;
    //            playerCollider.center = Vector3.zero;

    //        }
    //        else
    //        {
    //            moveSpeed = crouchSpeed;
    //            isCrouching = true;
    //            isSprinting = false;
    //            //change collider size
    //            playerCollider.height = playerManager.playerHeight * 0.5f;
    //            playerCollider.center = new Vector3(0f, -0.5f, 0f);
    //        }
    //    }
    //}

    //    */
    //}//crouch event

    //#endregion

    //#region sprint handler
    //[SerializeField] float SprintSpeed = 10f;
    //private void DoSprint(InputAction.CallbackContext context)
    //{
    //    // sprint code here
    //    //set move speed
    //    if (!playerController.GroundCheck() || playerState == Pstate.crouching)
    //        return;
    //    //sprint state
    //    if (playerState == Pstate.sprinting)
    //    {
    //        playerState = Pstate.walking;
    //        CurrMaxMoveSpeed = walkSpeed;
    //    }
    //    else
    //    {
    //        playerState = Pstate.sprinting;
    //        CurrMaxMoveSpeed = SprintSpeed;
    //    }
    //    /*

    //private void Sprinting()
    //{
    //    if (Input.GetKeyDown(playerManager.sprint))//toggle sprint
    //    {
    //        if (playerManager.grounded && !isCrouching)
    //        {
    //            if (!isSprinting)//&& !isCrouching && grounded)//if is not running then run
    //            {
    //                moveSpeed = sprintSpeed;
    //                isSprinting = true;
    //            }
    //            else//if is run then stop running
    //            {
    //                moveSpeed = walkSpeed;
    //                isSprinting = false;
    //            }
    //        }
    //    }
    //}
    //    */
    //}//sprint event

    //#endregion

    //#endregion

}
