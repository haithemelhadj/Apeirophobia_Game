using UnityEngine;
using UnityEngine.InputSystem;

public class CollectingAndInventory : MonoBehaviour
{

    public PLayer_Inputs playerInputs;
    //create each item as a public int variable
    public int ropePeice = 0;
    public int metalPiece = 0;
    public int rock = 0;

    public Transform playerObj;

    GameObject item = null;
    bool isNearItem = false;
    private void Awake()
    {
        playerInputs = new PLayer_Inputs();

    }
    public void Start()
    {

    }


    private void Update()
    {

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            //Debug.Log("item hit enter");
            isNearItem = true;
            item = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //if(other.la)
        if (other.CompareTag("Item"))
        {
            //Debug.Log("item hit exit");
            isNearItem = false;
            item = null;
        }
    }

    private void OnEnable()
    {
        playerInputs.Enable();
        playerInputs.Player.Collect.started += OnCollect;
    }
    private void OnDisable()
    {
        playerInputs.Player.Collect.started -= OnCollect;
        playerInputs.Disable();
    }


    private void OnCollect(InputAction.CallbackContext context)
    {
        Debug.Log("collect");
        if (isNearItem)
        {
            Debug.Log("item picked");
            Physics.Raycast(playerObj.position, item.transform.position - playerObj.position, out RaycastHit Hit);
            Vector3 normal = Hit.normal;
            normal.y = 0;
            playerObj.transform.forward = -normal;

            AddItem();
            RemoveItem();


        }
    }


    void RemoveItem()//remove item from the player's vision
    {
        item.SetActive(false);
    }

    void AddItem()//add item to inventory
    {

    }

}
