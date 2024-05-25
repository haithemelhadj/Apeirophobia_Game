using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerMovementTest1 : MonoBehaviour
{
    //player controller

    [Header("Components")] //player components
    public Rigidbody rb;
    public Animator animator;
    public Transform playerObj;
    public bool isGrounded = false;

    [SerializeField] Vector3 inputDir;

    #region other functions


    // mouse lock 
    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void MoveToOffset(Vector3 offset)
    {
        //used to move the player to the offset position
    }

    #region Ground Check

    // grounded check
    [SerializeField] LayerMask whatIsGround;
    [SerializeField] float extraScanDistance = 0.05f;
    [SerializeField] float playerHeight = 1f;
    private bool GroundCheck()
    {
        bool grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + extraScanDistance, whatIsGround + whatIsMovable);
        animator.SetBool("Grounded", grounded);
        return grounded;
    }

    #endregion

    #region Over Head CHeck

    // over head check
    private bool OverHeadCheck()
    {
        return Physics.Raycast(transform.position, Vector3.up, playerHeight * 0.5f + extraScanDistance);
    }

    #endregion

    #endregion

    #region Unity Functions

    //Gizmos draw distances
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        //grounded check
        Gizmos.DrawRay(transform.position, Vector3.down * (playerHeight * 0.5f + extraScanDistance));
        //over head check 
        Gizmos.DrawRay(transform.position, Vector3.up * (playerHeight * 0.5f + extraScanDistance));
        //front scan
        Gizmos.DrawRay(transform.position, playerObj.forward * (playerWidth * 0.5f + extraFrontScanDistance));
        //idk
        //Gizmos.DrawSphere(playerObj.transform.position + offsetPub, 0.1f);


    }
    private void Awake()
    {
        LockCursor();
        playerInputs = new PLayer_Inputs();
        CurrMaxMoveSpeed = walkSpeed;

    }
    private void Start()
    {

        //PauseMenu.SetActive(false);
    }

    private void Update()
    {
        //Debug.Log("my player pos : " + transform.position);
        PauseGame();
        inputDir = InputDir();
        isGrounded = GroundCheck();
        if (!isGrounded)
        {
            LastTouchGroundTime = Time.time;
        }
        else if (playerState == Pstate.jumping && rb.velocity.y <= 0)
        {
            //Debug.Log("landed");
            //playerState = Pstate.standing;
        }
        //Debug.Log("1" + transform.position);
        VSpeedControl();

        LedgeGrab();

        ClimbLadder();
        //Debug.Log("1" + transform.position);
        //CameraManager();

        AnimatorUpdate();
        RotatePlayer();
        //Debug.Log("1" + transform.position);
        MovementDrag();
        HSpeedControl();
        //Debug.Log("1" + transform.position);
    }
    private void FixedUpdate()
    {
        MovePlayer();
    }

    //player inputs
    [Header("Inputs")]
    public PLayer_Inputs playerInputs;
    private InputAction move;
    private void InputEventsSubscriptions()
    {
        playerInputs.Player.Jump.started += OnJump;
        playerInputs.Player.Grab.started += OnGrab;
        playerInputs.Player.Scroll.started += Scroll;
        playerInputs.Player.Throw.started += OnThrow;
        playerInputs.Player.Crouch.started += DoCrouch;
        playerInputs.Player.Sprint.started += DoSprint;
        playerInputs.Player.Collect.started += OnCollect;
        playerInputs.Player.LedgeClimb.started += LedgeClimb;
    }


    private void InputEventsUnsubscriptions()
    {
        playerInputs.Player.Jump.started -= OnJump;
        playerInputs.Player.Grab.started -= OnGrab;
        playerInputs.Player.Scroll.started -= Scroll;
        playerInputs.Player.Throw.started -= OnThrow;
        playerInputs.Player.Crouch.started -= DoCrouch;
        playerInputs.Player.Sprint.started -= DoSprint;
        playerInputs.Player.Collect.started -= OnCollect;
        playerInputs.Player.LedgeClimb.started -= LedgeClimb;
    }


    private void OnEnable()
    {
        InputEventsSubscriptions();
        move = playerInputs.Player.Move; //set player movement inputs 
        playerInputs.Player.Enable();
    }

    private void OnDisable()
    {
        InputEventsUnsubscriptions();
        playerInputs.Player.Disable();
    }

    #endregion

    #region animator system
    private void AnimatorUpdate()
    {
        animator.SetBool("Grounded", isGrounded);
        if (playerState == Pstate.pushing)
        {
            animator.SetFloat("VelocityH", inputDir.magnitude * CurrMaxMoveSpeed * MathF.Sign(move.ReadValue<Vector2>().y));
        }
        else if (playerState == Pstate.crouching)
        {
            animator.SetFloat("VelocityH", inputDir.magnitude);
        }
        else
        {
            animator.SetFloat("VelocityH", inputDir.magnitude * CurrMaxMoveSpeed);
        }
    }

    #endregion

    #region camera rotaion
    [SerializeField] Camera playerCamera;

    //get camera direction
    private Vector3 GetRightDirection()//return the vector 3 of the camera right 
    {
        Vector3 right = playerCamera.transform.right;
        right.y = 0;
        return right.normalized;
    }
    private Vector3 GetForwardDirection()//return the vector 3 of the camera forward
    {
        Vector3 forward = playerCamera.transform.forward;
        forward.y = 0;
        return forward.normalized;
    }

    #endregion

    #region camera states
    //set camera mods
    //camera state
    [Header("Camera State")]
    /*
    private Cstate cameraState;
    private enum Cstate
    {
        normal,
        sprinting,
        crouching,
        hanging
    private void CameraManager()
    {
        standFreeLook.SetActive(false);
        sprintFreeLook.SetActive(false);
        crouchFreeLook.SetActive(false);
        if (playerState == Pstate.crouching)
        {
            crouchFreeLook.SetActive(true);
        }
        else if (playerState == Pstate.standing && CurrMaxMoveSpeed == SprintSpeed) 
        {
            sprintFreeLook.SetActive(true);
        }
        else
        {
            standFreeLook.SetActive(true);
        }
    }
    }

    public GameObject standFreeLook;
    public GameObject crouchFreeLook;
    public GameObject sprintFreeLook;
    */
    //call function on state change and give correct number
    public GameObject[] cameras;
    public void CameraSystem(Pstate state)
    {
        return;
        int intValue = (int)state;
        for (int i = 0; i < cameras.Length; i++)
        {
            cameras[i].SetActive(false);

        }
        //if (intValue == 0 && CurrMaxMoveSpeed == SprintSpeed) intValue = cameras.Length - 1; // case of sprinting
        cameras[intValue].SetActive(true);
    }
    //---------------------------------- CameraSystem(playerState);

    #endregion


    #region player movement System

    // player state exists to check if the player is crouching or sprinting or walking, i don't like it, i think i'll go to a bool based system
    #region player states
    //player state
    [Header("Player State")]
    public Pstate playerState;
    public enum Pstate
    {
        standing,
        crouching,
        hanging,
        pushing,
        jumping,
        climbing,
        throwing,
    }
    #endregion

    #region player rotation handler

    //rotate player based on camera direction
    [SerializeField] public float rotationSpeed = 9f;
    private void RotatePlayer()
    {
        //smoothly change the player object rotation to the input direction
        if (inputDir != Vector3.zero && playerState != Pstate.pushing && !isClimbing)
        {
            playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
        }
    }

    #endregion

    #region inputs manager

    //player horizental input value
    private Vector3 InputDir()//return the horizontal input direction based on camera
    {
        Vector3 dir = Vector3.zero;
        if (isHanging) //can't move while hanging
        {
            return dir;
        }
        if (Time.time - LastTouchGroundTime < landingDuration && isGrounded) //can't move when landing
        {
            return dir;
        }

        if (isClimbing)
        {
            dir += move.ReadValue<Vector2>().y * Vector3.up;
            return dir;
        }
        if (playerState == Pstate.pushing)
        {

            if (move.ReadValue<Vector2>().y > 0)
                dir += move.ReadValue<Vector2>().y * playerObj.forward;
            return dir;
        }


        dir += move.ReadValue<Vector2>().x * GetRightDirection();
        dir += move.ReadValue<Vector2>().y * GetForwardDirection();
        return dir;
    }



    #endregion

    #region movement handler
    [Header("Movement Speed")]//player movement variables
    // horizontal movement
    [SerializeField] public float CurrMaxMoveSpeed;
    [SerializeField] float acceleration = 1;
    [SerializeField] float deceleration = 3;
    [SerializeField] public float walkSpeed = 5;

    private void MovePlayer()
    {
        Vector3 currentSpeed;
        if (inputDir != Vector3.zero)// will add more conditions soon
        {
            currentSpeed = Vector3.MoveTowards(rb.velocity, inputDir.normalized * CurrMaxMoveSpeed, acceleration);
        }
        else
        {
            currentSpeed = Vector3.MoveTowards(rb.velocity, Vector3.zero, deceleration);
        }
        if (isClimbing)//go up and down if climbing ladder
        {
            rb.velocity = new Vector3(0f, currentSpeed.y, 0f);
            return;
        }
        rb.velocity = new Vector3(currentSpeed.x, rb.velocity.y, currentSpeed.z);
    }



    // movement drag
    [SerializeField] float groundDrag = 2f;
    [SerializeField] float AirDrag = 3;
    private void MovementDrag()
    {
        if (isGrounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = AirDrag;
        }
    }

    //speed control
    private void HSpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        //limit  velocity if needed
        if (flatVel.magnitude > CurrMaxMoveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * CurrMaxMoveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    #endregion

    #region crouch handler
    [SerializeField] float CrouchSpeed = 2f;
    public CapsuleCollider capsuleCollider;
    private void DoCrouch(InputAction.CallbackContext context)
    {
        if (!isGrounded && (playerState != Pstate.standing || playerState != Pstate.crouching))
            return;

        //crouch state
        if (playerState == Pstate.crouching && !OverHeadCheck())
        {
            playerState = Pstate.standing;
            CameraSystem(playerState);
            CurrMaxMoveSpeed = walkSpeed;
            animator.SetBool("Crouching", playerState == Pstate.crouching);
            //change collider size
            capsuleCollider.height = playerHeight;//1
            capsuleCollider.center = new Vector3(0f, 0.5f, 0f);// 0,0.5,0 (+ radius = 0.2)

        }
        else
        {
            playerState = Pstate.crouching;
            CameraSystem(playerState);
            CurrMaxMoveSpeed = CrouchSpeed;
            animator.SetBool("Crouching", playerState == Pstate.crouching);
            Debug.Log("crouch");
            //change collider size
            capsuleCollider.height = playerHeight * 0.6f;//0.6
            capsuleCollider.center = new Vector3(0f, 0.3f, 0f);// 0,0.3,0 (+ radius = 0.2)

        }
    }

    #endregion

    #region sprint handler
    public float SprintSpeed = 10f;
    public bool canSprint;
    private void DoSprint(InputAction.CallbackContext context)
    {
        if (!canSprint)
        {
            return;
        }
        if (!isGrounded || playerState != Pstate.standing)
            return;
        //sprint state
        if (CurrMaxMoveSpeed == SprintSpeed)
        {
            CurrMaxMoveSpeed = walkSpeed;
        }
        else
        {
            CurrMaxMoveSpeed = SprintSpeed;
            CameraSystem(playerState);
        }
        playerState = Pstate.standing;
        CameraSystem(playerState);

    }//sprint event

    #endregion

    #endregion

    #region Jump System

    #region jump handler

    [Header("In Air")]
    public float jumpForce = 7;
    public float LastTouchGroundTime = 0;
    public float landingDuration = 0.14f;
    private void Jump()
    {
        //set state
        playerState = Pstate.jumping;
        CameraSystem(playerState);
        //ledge hang rest
        isHanging = false;
        rb.useGravity = true;
        capsuleCollider.enabled = true;
        animator.SetBool("Hanging", isHanging);
        //reset y velocity to 0
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        //give jump force impulse
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        //animator jump trigger
        animator.SetTrigger("Jump");
    }



    public void OnJump(InputAction.CallbackContext context) // jump event
    {

        if (playerState == Pstate.hanging)
        {
            Jump();
            return;
        }

        if (!isGrounded || playerState == Pstate.crouching || playerState == Pstate.pushing)
            return;

        if (Time.time - LastTouchGroundTime < landingDuration)
        {
            return;
        }
        Jump();
    }

    #endregion




    #region falling

    [SerializeField] float downGravityforce = 2f;
    //fall speed conrol
    public float maxFallSpeed = 10f;
    private void VSpeedControl()
    {
        Vector3 flatVel = new Vector3(0f, rb.velocity.y, 0f);
        if (flatVel.magnitude > maxFallSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * maxFallSpeed;
            rb.velocity = new Vector3(rb.velocity.x, limitedVel.y, rb.velocity.z);
        }
        else if (rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * downGravityforce * Time.deltaTime;
        }
    }
    #endregion


    #endregion

    #region Ledge Grab
    [Header("Ledge Grab")]
    public LayerMask whatIsLedge;
    public Vector3 LineDownStart;
    public Vector3 LineDownEnd;
    public float lineFrwdDistance = 0.3f;
    public float SlinePosDistance = 1f;
    public float ElinePosDistance = 0.3f;
    public bool isHanging;

    public Vector3 offsetPub;

    public Vector3 hangingPosition;
    void LedgeGrab()
    {
        if (rb.velocity.y < 0 && !isHanging && !isClimbing)
        {
            RaycastHit downHit;
            LineDownStart = (playerObj.transform.position + Vector3.up * SlinePosDistance) + playerObj.transform.forward * lineFrwdDistance;
            LineDownEnd = (playerObj.transform.position + Vector3.up * ElinePosDistance) + playerObj.transform.forward * lineFrwdDistance;
            Debug.DrawLine(LineDownStart, LineDownEnd, color: Color.red);
            Physics.Linecast(LineDownStart, LineDownEnd, out downHit, whatIsLedge);
            if (downHit.collider != null)
            {
                RaycastHit fwdHit;
                Vector3 lineFwdStart = new Vector3(playerObj.transform.position.x, downHit.point.y - 0.1f, playerObj.transform.position.z);
                Vector3 LineFwdEnd = new Vector3(playerObj.transform.position.x, downHit.point.y - 0.1f, playerObj.transform.position.z) + playerObj.transform.forward;
                Physics.Linecast(lineFwdStart, LineFwdEnd, out fwdHit, whatIsLedge);
                Debug.DrawLine(lineFwdStart, LineFwdEnd);
                if (fwdHit.collider != null)
                {
                    //freeze player
                    rb.useGravity = false;
                    rb.velocity = Vector3.zero;
                    //set hanging state
                    playerState = Pstate.hanging;
                    isHanging = true;
                    //set camera state
                    CameraSystem(playerState);
                    //set animation
                    animator.SetBool("Hanging", isHanging);

                    //get ground ledge
                    Vector3 hangPos = new Vector3(fwdHit.point.x, downHit.point.y, fwdHit.point.z);
                    Debug.Log("hangpos1=" + hangPos);


                    //get offset
                    offsetPub = playerObj.forward * -playerWidth * 0.5f + Vector3.up * -0.1f;
                    Debug.Log("offset= " + offsetPub);
                    //move player to ledge
                    transform.position = hangPos + offsetPub;
                    //rotate player 
                    playerObj.transform.forward = -fwdHit.normal;

                    //save hanging position to fix if ruined
                    hangingPosition = transform.position;
                    //set other constraints



                }
            }
        }
    }


    private void LateUpdate()
    {
        LedgeGrabBugFix();
    }

    private void LedgeGrabBugFix()
    {
        if (playerState == Pstate.hanging)
        {
            if (transform.position.y != hangingPosition.y)
            {
                transform.position = hangingPosition;
            }
            Debug.Log(transform.position);
        }
    }


    #endregion

    #region Ledge Climb

    private void LedgeClimb(InputAction.CallbackContext context)
    {
        return;
        if (playerState == Pstate.hanging)
        {
            animator.applyRootMotion = true;
            animator.SetTrigger("LedgeClimb");
        }
    }


    #endregion

    #region Inventory

    //if throw button pressed and is standing ( or is not jumping)
    //set throwing state, set animation and camera
    //add function to select item to use
    //add throwing trajictory when aiming
    //on release spawn item and give it a throwing force and reset camera and state
    [Header("Inventory")]
    public GameObject[] inventory; // inventory

    [Header("Collect Item")]
    public GameObject item = null; // collected item
    public bool isNearItem = false;

    //public List<GameObject> inventoryList;

    [Header("Inventory Scroll")]
    public float scrollSpeed = 3;
    public float selector = 0;
    private float sum = 0;

    [Header("Inventory Visual")]
    //public Item itemScriptable;
    public GameObject selectorImg;
    public GameObject[] toolbarSlots; // visal place holder for items
    public GameObject[] itemIcons; // visal place holder for item icons

    private void InventorySelect()
    {
        //on scroll add to selector
        //if selector is bigger than inventory length set it to 0
    }

    private void OnThrow(InputAction.CallbackContext context)
    {
        //hold to aim doesnt work so ill do it like the other ones
        /*
        Debug.Log("left mouse");
        //if preforming start aiming 
        if(context.performed)
        {
        Debug.Log("Preformed");
            if(playerState==Pstate.standing)
            {
                //set player state
                playerState = Pstate.throwing;
                //CameraSystem(playerState);
                //set animator
                animator.SetTrigger("Throw");
            }
        }
        else if (context.canceled)//if canceled throw item 
        {
            playerState = Pstate.standing;
            animator.SetTrigger("Release");

        }
        */
    }
    private void Scroll(InputAction.CallbackContext context)
    {
        float scroll = context.ReadValue<float>();
        sum += scroll / scrollSpeed;
        if (sum >= 120)
        {
            sum -= 120;
            selector--;
        }
        else if (sum <= -120)
        {
            sum += 120;
            selector++;
        }

        if (selector < 0)
        {
            selector = toolbarSlots.Length - 1;
        }
        else if (selector >= toolbarSlots.Length)
        {
            selector = 0;
        }
        //Debug.Log("selector2= " + selector);
        selectorImg.transform.position = toolbarSlots[(int)selector].transform.position;
    }

    #endregion

    #region Collect Item


    private void OnCollect(InputAction.CallbackContext context)
    {
        if (isNearItem)
        {
            //rotate player to object
            Physics.Raycast(playerObj.position, item.transform.position - playerObj.position, out RaycastHit Hit);
            Vector3 normal = Hit.normal;
            normal.y = 0;
            playerObj.transform.forward = -normal;
            //reset near item cuz trigger exit is not called
            isNearItem = false;

            //add animation

            //check if player is looking at it with vector3.dot;

            AddItem(item);
        }
    }

    public void AddItem(GameObject newItem)//add item to inventory
    {
        //check for an empty slot
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] == null)
            {
                //add item
                inventory[i] = newItem;
                //show item
                //toolbarSlots[i].GetComponentInChildren<SpriteRenderer>().sprite=newItem.GetComponent<Collectable>().itemImage;
                itemIcons[i].SetActive(true);
                itemIcons[i].transform.position = toolbarSlots[i].transform.position;
                itemIcons[i].GetComponent<Image>().sprite = newItem.GetComponent<Collectable>().itemImage;
                //remove item 
                RemoveItem(newItem);
                return;
            }
        }
    }

    public void RemoveItem(GameObject newItem)//remove item from the player's vision
    {
        Debug.Log("item removed");
        newItem.SetActive(false);
    }



    private void GoingNearItem(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            isNearItem = true;
            item = other.gameObject;
        }
        else if (other.CompareTag("Item"))
        {
            Debug.Log("ladder hit enter");
            isNearLadder = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            isNearItem = false;
            item = null;
        }
        else if (other.CompareTag("Ladder"))
        {
            Debug.Log("ladder hit exit");
            isNearItem = false;
        }
    }

    #endregion

    #region Climb Ladder
    [Header("Ladder Climb")]
    public bool isNearLadder = false;
    public float ClimbingSpeed = 2f;
    public bool isClimbing = false;
    public float upExtraDistance = 0.4f;
    public Vector3 upDistance;//0,-0.4,0
    public Vector3 lastGrabLadderDirection;

    private void ClimbLadder()
    {
        if (!isClimbing)//if is not climbing
        {
            if (playerState != Pstate.standing && playerState != Pstate.jumping)
            {
                //can only climb ladder from standing state
                return;
            }
            //check if there is an object in front of the player
            Debug.DrawRay(transform.position + upDistance, playerObj.forward, Color.green);
            if (Physics.Raycast(transform.position + upDistance, playerObj.forward, out RaycastHit hit, playerWidth * 0.5f + extraFrontScanDistance))
            {
                //check if there is a ladder in front of the player
                if (hit.transform.TryGetComponent(out Ladder ladder) && (move.ReadValue<Vector2>().y > 0f))
                {
                    //Grab ladder 
                    GrabLadder(playerObj.forward);
                    //set rotation
                    playerObj.forward = -hit.normal;
                    //move to offeset
                    //MoveToOffset(Vector3.zero);//------------------------------------------------------------------------------------------
                }
            }
        }
        //if is already climbing
        else
        {
            animator.SetFloat("Climb Dir", move.ReadValue<Vector2>().y);
            //check if the player is still on the ladder, if not drop the ladder
            if (Physics.Raycast(transform.position + upDistance, lastGrabLadderDirection, out RaycastHit hitcast, playerWidth * 0.5f + extraFrontScanDistance))
            {
                if (!hitcast.transform.TryGetComponent(out Ladder ladder))
                {
                    DropLadder();
                }
            }
            else
            {
                DropLadder();
            }

            //if the player is moving down, check if he reached the bottom of the ladder
            if (move.ReadValue<Vector2>().y < 0f)
            {
                if (Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + extraScanDistance, whatIsGround))
                {
                    DropLadder();
                }
            }
        }
    }



    private void GrabLadder(Vector3 lastGrabLadderDirection)
    {
        //climb bool
        isClimbing = true;
        //gravity
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        //set last grab ladder direction //extra?
        this.lastGrabLadderDirection = lastGrabLadderDirection;
        //change grounded value
        isGrounded = true;
        //set other values 
        //animation
        animator.SetBool("Climbing", isClimbing);
        //move speed
        CurrMaxMoveSpeed = ClimbingSpeed;
        // player state
        playerState = Pstate.climbing;
        CameraSystem(playerState);
    }
    private void DropLadder()
    {
        //climb bool
        isClimbing = false;
        //gravity
        rb.useGravity = true;
        inputDir = Vector3.zero;
        //animation
        animator.SetBool("Climbing", isClimbing);
        //move speed
        CurrMaxMoveSpeed = walkSpeed;
        //player state
        playerState = Pstate.standing;
        CameraSystem(playerState);
    }

    #endregion

    #region Puch & Pull

    //ray cast in front of the player to check for a movable object
    //if there is allow for holding object and enter moving object state if a button is pressed
    //when in moving object state only allow for 1 axis movement, get it's value and turn it into blend tree animation
    //
    // grounded check
    [Header("Move Object")]
    [SerializeField] LayerMask whatIsMovable;
    [SerializeField] float extraFrontScanDistance = 0.1f;
    [SerializeField] float playerWidth = 0.4f;

    [SerializeField] float pushSpeed = 1;
    [SerializeField] Transform movedObject;
    public Vector3 movedObjectOffset;
    private void OnGrab(InputAction.CallbackContext context)
    {
        //check if there is a movable object in front of the player
        RaycastHit hit;
        bool canMove = Physics.Raycast(transform.position, playerObj.forward, out hit, playerWidth * 0.5f + extraFrontScanDistance, whatIsMovable);
        //if player is already pushing stop pushing
        if (playerState == Pstate.pushing)
        {
            //set player state
            playerState = Pstate.standing;
            CameraSystem(playerState);
            //remove movement restrictions
            CurrMaxMoveSpeed = walkSpeed;

            //set animator back to walking
            animator.SetTrigger("Stand");
            //remove parent
            movedObject.parent = null;
            movedObject = null;


        }
        else if (canMove && isGrounded)
        {
            //move to offeset
            //MoveToOffset(Vector3.zero);//------------------------------------------------------------------------------------------

            //move object with player
            movedObject = hit.transform;
            //set player state
            playerState = Pstate.pushing;
            CameraSystem(playerState);
            //set animator
            animator.SetTrigger("Grab");
            //restrict movement
            CurrMaxMoveSpeed = pushSpeed;
            //set rotation
            playerObj.forward = -hit.normal;

            //get offset position (x,player y, raycast z)

            transform.position += movedObjectOffset.z * playerObj.forward;

            //make the object the child of the player
            movedObject.SetParent(this.transform);

        }
    }

    #endregion

    #region pause menu
    public GameObject PauseMenu;
    public GameObject resumeButton;
    public GameObject checkpointButton;



    public void PauseGame()
    {
        if (gameOver)
            return;


        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            Time.timeScale = Time.timeScale == 0f ? 1f : 0f;
            Cursor.visible = !Cursor.visible;
            Cursor.lockState = Cursor.lockState == CursorLockMode.None ? CursorLockMode.Locked : CursorLockMode.None;
            //bool isActive = PauseMenu.active;
            PauseMenu.SetActive(!PauseMenu.activeSelf);
            resumeButton.SetActive(PauseMenu.activeSelf);

        }
    }
    #endregion

    #region Loss

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("LowestPoint"))
        {
            GameOver();
            //PlayerLoss.Loss();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        GoingNearItem(other);
        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("LowestPoint"))
        {
            GameOver();
            //PlayerLoss.Loss();
        }
    }


    public static bool gameOver;
    public void GameOver()
    {
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        PauseMenu.SetActive(true);
        resumeButton.SetActive(false);
        //checkpointButton.SetActive(true);
        gameOver = true;
    }
    #endregion

    #region extra 
    #region Interactions




    //speak with npc if there is

    //open door if there is

    #endregion

    #endregion

    //add items to inventory
    //use scriptable objects to add count and usabilty 
    //add list

    //fix ledge hang position +++
    //cat walk ++++++
    //fix push and pull position+++++

    //add ledge climb animation and logic ==== i don't want it anymore
    //add throw item logic

    //create camera modes system
    //camera switching rotates the player, must fix or remove
    // remove booleans that are can be replaced with player states, they start with "is...."
    //create moveto offset function
    //look at variables from inspector
    //make player can go on slopes and diagonal ladders

    //split code in diffrent scripts

    /*
     * when changing animations remember to
     * add events at the end of animation to restate player state
     * fix landing pose timing
     * 

    */
}
