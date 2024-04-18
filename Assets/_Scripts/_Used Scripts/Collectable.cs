using UnityEngine;
using UnityEngine.InputSystem;

public class Collectable : MonoBehaviour
{
    public Transform playerObj;

    #region Collect Item
    public Item Scriptable;
    public Sprite itemImage;
    [Header("Collect Item")]
    public GameObject item = null;
    public bool isNearPlayer = false;

    private void OnCollect(InputAction.CallbackContext context)
    {
        if (isNearPlayer)
        {
            Physics.Raycast(playerObj.position, item.transform.position - playerObj.position, out RaycastHit Hit);
            Vector3 normal = Hit.normal;
            normal.y = 0;
            playerObj.transform.forward = -normal;
            isNearPlayer = false;

            //add animation

            //check if player is looking at it with vector3.dot;

            AddItem();
            RemoveItem();


        }
    }


    void RemoveItem()//remove item from the player's vision
    {
        item.SetActive(false);
    }

    //create each item as a public int variable
    public int ropePeice = 0;
    public int metalPiece = 0;
    public int rock = 0;
    void AddItem()//add item to inventory
    {
        //nkamelha ba3d ma aziz ye5dem l items scriptable object
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isNearPlayer = true;
            item = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isNearPlayer = false;
            item = null;
        }
    }

    #endregion
}
