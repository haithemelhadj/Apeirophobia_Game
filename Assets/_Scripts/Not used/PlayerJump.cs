using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class PlayerJump : MonoBehaviour
{
    [Range(0f, 0.2f)] public float extraScanDistance = 0.1f;
    /*
    public Text velX;
    public Text velY;
    public Text velZ;
    void Debugs()
    {
        //Debug.Log("vel.x = " + rb.velocity.x);
        velX.text = "vel.x = " + playerManager.rb.velocity.x;
        //Debug.Log("vel.y = " + rb.velocity.y);
        velY.text = "vel.y = " + playerManager.rb.velocity.y;
        //Debug.Log("vel.z = " + rb.velocity.z);
        velZ.text = "vel.z = " + playerManager.rb.velocity.z;
    }
    */
    [Header("Components")]
    public PlayerManager playerManager;
    //public Rigidbody rb;
    public Transform orientation;

    [Header("Jump")]
    public float jumpForce = 10;
    public int extraJumps = 1;
    public int leftExtraJumps;

    [Header("Falling")]
    public float maxFallSpeed = 10;
    public float downGravityforce = 1;

    [Header("Jump Buffer")]
    public float jumpTimer = 1f;
    public float jumpBufferCooldown;

    [Header("Cyote Time")]
    public float cyoteTimer = 1f;
    public float cyoteCoooldown;

    [Header("Ledge Hanging")]
    //public bool isHanging;
    [Range(0f,3f)] public Vector3 offSet;
    /*
    public Transform underHead;
    public Transform overHead;
    */

    [Header("Ground Check")]
    //public float playerHeight = 2;
    //public LayerMask whatIsGround;
    //public bool grounded;
    //public bool isUnder;
    public float groundDrag;
    public bool isFalling;


    //-----------------------------------------------------------------------------------
    #region start/update/fixedupdate
    private void Start()
    {
        
    }
    private void Update()
    {
        GroundCheck();
        DoubleJump();
        JumpBuffer();
        //CyoteTime();
        LedgeGrab();
        //LedgeDetection();

        Inputs();
    }
    private void FixedUpdate()
    {
        Friction();
        FallControl();
        FallClamp();
        //Debugs();
    }
    #endregion
    //-----------------------------------------------------------------------------------    

    #region functions
    
    private void Inputs()
    {
        // get when jump key is pressed
        if (jumpBufferCooldown > 0)  //Input.GetKey(jumpKey)
        {
            if (playerManager.grounded || leftExtraJumps > 0 || playerManager.isHanging )  //(cyoteCoooldown > 0 || leftExtraJumps > 0) // grounded is put temporarly until cyote time is fixed || grounded
            {
                Jump();
            }
        }
    }
    

    private void Jump()
    {
        //if player is hanging, jump from ledge
        if(playerManager.isHanging)
        {
            playerManager.isHanging = false;
            playerManager.rb.useGravity = true;
        }

        //if player is in air then he has less jumps
        if (!playerManager.grounded && cyoteCoooldown < 0)
            leftExtraJumps--;        
        //reset y velocity to 0
        playerManager.rb.velocity = new Vector3(playerManager.rb.velocity.x, 0f, playerManager.rb.velocity.z);
        //give jump force impulse
        playerManager.rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        //reset cyote time
        //cyoteCoooldown = 0f;
        //reset jump buffer time 
        jumpBufferCooldown = 0f;

    }

    private void Friction()
    {
        //handle drag
        if (playerManager.grounded)
            playerManager.rb.drag = groundDrag;
        else
            playerManager.rb.drag = 0;
    }
    private void DoubleJump()
    {
        if (playerManager.grounded)
            leftExtraJumps = extraJumps;
    }
    private void FallControl()
    {
        if(!playerManager.grounded)
        {
            // early fall + faster fall
            // if player is moving down or jump button released early, apply extra gravity
            if ((playerManager.rb.velocity.y > 0 && !Input.GetKey(playerManager.jumpKey)) || playerManager.rb.velocity.y < 0) 
            {
                //apply extra gravity to fall faster
                playerManager.rb.AddForce(downGravityforce * Vector3.down, ForceMode.VelocityChange);
            }
        }
    }
    private void FallClamp()
    {
        //clamp falling speed
        if (playerManager.rb.velocity.y < 0)
        {
            playerManager.rb.velocity = new Vector3(playerManager.rb.velocity.x, Mathf.Max(playerManager.rb.velocity.y, -maxFallSpeed), playerManager.rb.velocity.z);
            isFalling = true;
        }
        else
        {
            isFalling = false;
        }
    }
    private void JumpBuffer()
    {
        //jump buffer : if player presses jump button before landing, jump as soon as grounded
        if (Input.GetKeyDown(playerManager.jumpKey))// && !grounded)
        {
            //jumpBuffer = true;
            jumpBufferCooldown = jumpTimer;
        }
        else
        {
            jumpBufferCooldown -= Time.deltaTime;
        }
    }
    #endregion
    private void GroundCheck()
    {
        //head check
        //isUnder = Physics.Raycast(transform.position, Vector3.up, playerHeight * 0.5f + extraScanDistance);
        ////ground check
        //playerManager.grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + extraScanDistance, whatIsGround);
    }

    #region Ledge Grab
    public Vector3 LineDownStart;
    public Vector3 LineDownEnd;
    void LedgeGrab()
    {
        if (playerManager.rb.velocity.y < 0 && !playerManager.isHanging && !playerManager.isUnder)
        {
            RaycastHit downHit;
            LineDownStart = (transform.position + Vector3.up * 1.5f) + transform.forward;
            LineDownEnd = (transform.position + Vector3.up * 0.7f) + transform.forward;
            Physics.Linecast(LineDownStart, LineDownEnd, out downHit, playerManager.whatIsGround);
            Debug.DrawLine(LineDownStart, LineDownEnd);
            if (downHit.collider != null)
            {
                RaycastHit fwdHit;
                Vector3 lineFwdStart = new Vector3(transform.position.x, downHit.point.y - 0.1f, transform.position.z);
                Vector3 LineFwdEnd = new Vector3(transform.position.x, downHit.point.y - 0.1f, transform.position.z) + transform.forward;
                Physics.Linecast(lineFwdStart, LineFwdEnd, out fwdHit, playerManager.whatIsGround);
                Debug.DrawLine(lineFwdStart, LineFwdEnd);
                if (fwdHit.collider != null)
                {
                    //freeze player
                    playerManager.rb.useGravity = false;
                    playerManager.rb.velocity = Vector3.zero;
                    //set hanging state
                    playerManager.isHanging = true;
                    //reset jump conditions
                    leftExtraJumps = extraJumps;

                    //set animation

                    //move player to ledge
                    Vector3 hangPos = new Vector3(fwdHit.point.x, downHit.point.y, fwdHit.point.z);
                    Vector3 offset = transform.forward * -0.1f + transform.up * -1f;
                    hangPos += offset;
                    transform.position = hangPos;
                    transform.forward = -fwdHit.normal;
                    //set other constraints

                }
            }
            //bugs in this code:
             //no gravity reset+
             //can move while hanging+ can't even move sideways
             //does it work on walls?+
             //player rotation?+
             //missing animation
             //has a bug when holding jump while hanging
        }
    }
    #endregion

    #region comments
    /*
    public void LedgeDetection()
    {
        if (isFalling)
        {
            Debug.Log("isFalling");
            //bool underHeadHit = Physics.Raycast(underHead.position, Vector3.forward, 1f);
            Ray underHeadHit = new Ray(underHead.position, orientation.forward);
                if (Physics.Raycast(underHeadHit, out RaycastHit overHit, 1f))
            {
                Debug.Log("ray cast 1");

                Ray overHeadHit = new Ray(underHead.position, Vector3.down);
            if (Physics.Raycast(underHeadHit, out RaycastHit underHit, 1f))
                {
                Debug.Log("ray cast 2");
                    //set hanging state
                    isHanging = true;
                    //freeze position
                    rb.useGravity = false;
                    rb.velocity = Vector3.zero;
                    //reset jump
                    leftExtraJumps = extraJumps;
                    //move player to ledge
                    Vector3 ledgePos = new Vector3(underHit.point.x, overHit.point.y, underHit.point.z);
                    transform.position = ledgePos + offSet;
                }
            }
            else if(!isHanging)
            {
                isHanging = false;
                //freeze position
                rb.useGravity = true;
            }
        }
    */
    /*
        //grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + extraScanDistance, whatIsGround);

        //Ray overHeadHit =  ;//Physics.Raycast(overHead.position, Vector3.forward, 1f);
        //bool underHeadHit = Physics.Raycast(underHead.position, Vector3.forward, 1f);

        Ray overHeadHit = new Ray(overHead.position, orientation.forward); 
        //Physics.Raycast(overHeadHit, out RaycastHit overHit);
        Ray underHeadHit = new Ray(overHead.position, orientation.forward);
        Debug.DrawRay(overHead.position, orientation.forward, Color.red, 1f);
        Debug.DrawRay(underHead.position, orientation.forward, Color.red, 1f);
        //if(underHead && !overHead && isFalling)
        if (Physics.Raycast(underHeadHit, out RaycastHit underHit, 1f) && Physics.Raycast(overHeadHit, out RaycastHit overHit, 1f) && isFalling) 
        {
            Debug.Log("is hanging");
            Vector3 ledgePos = Vector3.zero; // is the x horizontal of the player, the forward of the player when hitting the ledge, and the y is the hight of the ledge
        //Physics.Raycast(underHeadHit, out RaycastHit underHit);

            ledgePos.z = underHit.point.z;// this is the z
            Ray hightCheck = new Ray(new Vector3(underHit.point.x,overHead.position.y,underHit.point.z), Vector3.down);
            Physics.Raycast(underHeadHit, out RaycastHit hightCheckHit);
        Debug.DrawRay(new Vector3(underHit.point.x, overHead.position.y, underHit.point.z), Vector3.down, Color.blue, 1f);
            ledgePos.y = hightCheckHit.point.y;
            ledgePos.x = underHit.point.x;
            rb.constraints = RigidbodyConstraints.FreezeAll;

        }
        /*
    }
    */
    /*
    private void CyoteTime()
    {
        //cyote time 
        if (grounded)
        {
            cyoteCoooldown = cyoteTimer;
            //Invoke(nameof(ResetCyoteFull), 1f);
            //Debug.Log("cyote rest full");
        }
        //cyote time cooldown
        else 
        {
            cyoteCoooldown -= Time.deltaTime;
        }
    }
    */
    /*
    void ResetCyoteFull()
    {
    }
    */
    /*
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == whatIsGround)
        {
            Debug.Log("grounded = true");
            grounded = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == whatIsGround)
        {
            Debug.Log("grounded = false");
            grounded = false;
        }
    }
    */

    #endregion

    private void OnDrawGizmos()
    {
        /*
        Gizmos.color = Color.blue;
        //Gizmos.DrawRay(transform.position, Vector3.down * (playerHeight * 0.5f + extraScanDistance));//, Color.red
        Gizmos.DrawRay(underHead.position, Vector3.forward * 1f);//, Color.red
        Gizmos.DrawRay(overHead.position, Vector3.down * 1f);//, Color.red
        */
    }

    /*double information in scripts:
     * grounded and ground check
     * keybindings
     * 
     * buffer jump works weird
     * 
    */
    /*TO DO
     * add player movement state
     * coyote time still has a problem
     * 
     * double jump
     * early fall- done
     * jump buffering-done
     * clamp falling speed-done
     * //hold crouch to stay on ledge
     * 
    */
}
