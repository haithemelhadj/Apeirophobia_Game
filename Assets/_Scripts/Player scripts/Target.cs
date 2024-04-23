using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Target : MonoBehaviour
{
    public LayerMask playerMask;
    public float length;
    public GameObject UI;
    public PlayerMovementTest1 PlayerScript;
    private GameObject selectedItem;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f,0.5f,0));
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction * length , Color.red);
        if (Physics.Raycast(ray, out hit, length, ~playerMask))
        {
            Debug.Log(hit.collider.name);
            if (hit.collider.tag == "Item")
            {
                UI.SetActive(true);
                if (Input.GetKeyDown(KeyCode.E))
                {
                    PlayerScript.item = hit.collider.gameObject;
                    PlayerScript.AddItem(hit.collider.gameObject);
                    //Destroy(hit.collider.gameObject);
                    hit.collider.gameObject.SetActive(false);
                }
            }
            

        }
        else
        {
            UI.SetActive(false);
        }
        selectedItem = PlayerScript.inventory[(int)PlayerScript.selector];
        if (selectedItem != null)
        {
            Debug.Log(selectedItem.name);
        }
    }
  
}
