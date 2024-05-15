using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using Cinemachine;
using UnityEngine.WSA;
using Unity.VisualScripting;

public class Target : MonoBehaviour
{
    public LayerMask playerMask;
    public float length;
    public GameObject UI;
    public PlayerMovementTest1 PlayerScript;
    private GameObject selectedItem;
    public GameObject[] images;
    public int numOfImgsDone;
    public GameObject door;
    public GameObject PuzzleUI;
    public GameObject level2door;
    public Animator dooranim;
    public Lever leverScript;
    public CinemachineVirtualCamera doorcam;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        selectedItem = PlayerScript.inventory[(int)PlayerScript.selector];
        if (selectedItem != null)
        {
            Debug.Log(selectedItem.name);
            Debug.Log(selectedItem.gameObject.tag);
        }
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
            }else if(hit.collider.tag == "Moveable")
            {
                UI.SetActive(true);
            }else if(hit.collider.tag == "Frames")
            {
                UI.SetActive(true);
                if (Input.GetKeyDown(KeyCode.E))
                {
                if(selectedItem != null)
                {
                        if (selectedItem.name == "1")
                        {
                            images[0].SetActive(true);
                            RemoveCurrItemFromInventory();
                            numOfImgsDone++;
                            CheckToOpenDoor();
                        }
                        else if (selectedItem.name == "2")
                        {
                            images[1].SetActive(true);
                            RemoveCurrItemFromInventory();
                            numOfImgsDone++;
                            CheckToOpenDoor();
                        }
                        else if (selectedItem.name == "3")
                        {
                            images[2].SetActive(true);
                            RemoveCurrItemFromInventory();
                            numOfImgsDone++;
                            CheckToOpenDoor();
                        }
                        else if (selectedItem.name == "4")
                        {
                            images[3].SetActive(true);
                            RemoveCurrItemFromInventory();
                            numOfImgsDone++;
                            CheckToOpenDoor();
                        }
                        else if (selectedItem.name == "5")
                        {
                            images[4].SetActive(true);
                            RemoveCurrItemFromInventory();
                            numOfImgsDone++;
                            CheckToOpenDoor();
                        }
                }
                            

                }
            }
            else if (hit.collider.tag == "lockedDoor")
            {
                UI.SetActive(true);
                if (Input.GetKeyDown(KeyCode.E))
                {
                    PuzzleUI.SetActive(true);
                    UnityEngine.Cursor.lockState = CursorLockMode.None;
                }
            } else if(hit.collider.tag == "lever")
            {
                if (!leverScript) return;
                UI.SetActive(true);
                //play lever animation
                if (Input.GetKeyDown(KeyCode.E) && !leverScript.playingLeverAnimation)
                {
                leverScript.playingLeverAnimation = true;
                    //play door animation
                    Invoke("activateCam", 1.5f);
                    Invoke("OpenDoor", 3f);
                    Invoke("disableCamera", 6.5f);
                }
                
            }
        }
        else
        {
            UI.SetActive(false);
        }
    }
  
    private void CheckToOpenDoor()
    {
        if(numOfImgsDone>=5)
        {
            door.SetActive(false);
        }
    }

    private void RemoveCurrItemFromInventory()
    {
        Debug.Log((int)PlayerScript.selector);
        PlayerScript.inventory[(int)PlayerScript.selector] = null;
        PlayerScript.itemIcons[(int)PlayerScript.selector].SetActive(false);
    }
    public void OpenDoor()
    {
        dooranim.SetFloat("speed", 1f);
    }
    public void activateCam()
    {
        doorcam.Priority = 11;
    }
    public void disableCamera()
    {
        doorcam.Priority = 9;
    }
}
