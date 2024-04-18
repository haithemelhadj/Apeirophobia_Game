using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    #region Declarations
    [Header("Components")]
    public PlayerManager playerManager;
    public Transform orientation;
    public CapsuleCollider playerCollider;
    //public Material material;

    [Header("Movement")]
    public float moveSpeed;

    public float airMultiplier;
    public float sprintSpeed;
    public float walkSpeed;
    public float crouchSpeed;

    public bool isSprinting;
    public bool isCrouching;

    [Header("Crouching")]
    //public Transform headObject;


    [Header("Ground Check")]
    //[Range(0f, 0.2f)] public float extraScanDistance = 0.05f;
    //public float playerHeight = 2;
    //public LayerMask whatIsGround;
    //private bool grounded;



    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    //public Rigidbody rb;


    [Header("Player State")]
    public playerstate playerState;
    public enum playerstate
    {
        walking,
        sprinting,
        crounching,
        inAir
    }

    /*
    public float maxSpeed;
    public float currentSpeed;
    public float acceleration;
    public float decelration;
    */
    #endregion


    //-----------------------------------------------------------------------------------
    #region start/update/fixedupdate
    private void Start()
    {
        //rb = GetComponent<Rigidbody>();
        playerManager.rb.freezeRotation = true;

    }

    private void Update()
    {
        MyInput();
        //Debug.Log("speed 1 is " + moveSpeed);
        Crouching();
        //Debug.Log("speed 2 is " + moveSpeed);
        Sprinting();
        //Debug.Log("speed 3 is " + moveSpeed);
        speedControl();
        //Debug.Log("speed 4 is " + moveSpeed);
        //AnimationControl();



        /*
        if (grounded && Input.GetKeyDown(sprint) && verticalInput > 0 && ) 
        {
            //issprinting true
            playerState = playerstate.sprinting;
        }
        else if(grounded && Input.GetKeyDown(crouch))
        {
            //iscrouching true
            playerState = playerstate.crounching;
        }
        else if (playerState == playerstate.crounching && !isUnder)
        {

        }
        else
        {
            //iswalking true
            playerState = playerstate.walking;
        
        }
        */
    }
    private void FixedUpdate()
    {
        MovePlayer();
    }
    #endregion
    //-----------------------------------------------------------------------------------
    #region debugs
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector3.down * (playerManager.playerHeight * 0.5f + playerManager.extraScanDistance));//, Color.red
        Gizmos.DrawRay(transform.position, Vector3.up * playerManager.extraScanDistance);//, Color.red
    }
    #endregion

    #region Animations
    /*
    public void AnimationControl()
    {
        playerManager.animator.SetBool("isSprinting", isSprinting);
        playerManager.animator.SetBool("isCrouching", isCrouching);
    }
    */
    #endregion

    #region Input

    // get player input
    private void MyInput()
    {
        if (playerManager.isHanging)
        {
            return;
        }
        //get x,z movement direction
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

    }
    private void Crouching()
    {
        if (Input.GetKeyDown(playerManager.crouch) && playerManager.grounded)
        {
            if (isCrouching && !playerManager.isUnder)
            {
                moveSpeed = walkSpeed;
                isCrouching = false;
                //change collider size
                playerCollider.height = playerManager.playerHeight;
                playerCollider.center = Vector3.zero;

            }
            else
            {
                moveSpeed = crouchSpeed;
                isCrouching = true;
                isSprinting = false;
                //change collider size
                playerCollider.height = playerManager.playerHeight * 0.5f;
                playerCollider.center = new Vector3(0f, -0.5f, 0f);
            }
        }
    }

    private void Sprinting()
    {
        if (Input.GetKeyDown(playerManager.sprint))//toggle sprint
        {
            if (playerManager.grounded && !isCrouching)
            {
                if (!isSprinting)//&& !isCrouching && grounded)//if is not running then run
                {
                    moveSpeed = sprintSpeed;
                    isSprinting = true;
                }
                else//if is run then stop running
                {
                    moveSpeed = walkSpeed;
                    isSprinting = false;
                }
            }
        }
    }
    #endregion


    #region Control PlayerState

    /*
    private void playerStateControl()
    {

        //set player playerState before moving
        if(!grounded)
        {
            playerState = playerstate.inAir;
        }
        else if(Input.GetKey(sprint))
        {
            playerState = playerstate.sprinting;
        }
        else if(Input.GetKey(crouch))
        {
            playerState = playerstate.crounching;
        }
        else
        {
            playerState = playerstate.walking;
        }
        //player playerState switch 
        switch(playerState)
        {
            case playerstate.walking:
                moveSpeed = walkSpeed;
                break;
            case playerstate.sprinting:
                moveSpeed = sprintSpeed;
                break;
            case playerstate.crounching:
                moveSpeed = crouchSpeed;
                break;
            case playerstate.inAir:
                moveSpeed = 10f;
                break;
        }        
    }
    */
    #endregion


    #region Movement
    //player movement logic
    private void MovePlayer()
    {
        //calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // on ground
        if (playerManager.grounded)
            playerManager.rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        // in air
        else
            playerManager.rb.AddForce(moveDirection.normalized * moveSpeed * airMultiplier * 10f, ForceMode.Force);

    }

    //limit player speed to never go over what is set in moveSpeed
    private void speedControl()
    {
        Vector3 flatVel = new Vector3(playerManager.rb.velocity.x, 0f, playerManager.rb.velocity.z);
        //limit  velocity if needed
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            playerManager.rb.velocity = new Vector3(limitedVel.x, playerManager.rb.velocity.y, limitedVel.z);
        }
    }
    #endregion

}
