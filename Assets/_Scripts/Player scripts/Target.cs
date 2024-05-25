using Cinemachine;
using UnityEngine;

public class Target : MonoBehaviour
{
    public LayerMask playerMask;
    public float length;
    public GameObject UI;
    public PlayerMovementTest1 PlayerScript;
    public GameObject selectedItem;
    public GameObject[] images;
    public int numOfImgsDone;
    public GameObject door;
    public GameObject PuzzleUI;
    public GameObject level2door;
    public Animator dooranim;
    public Lever leverScript;
    public CinemachineVirtualCamera doorcam;
    public GameObject[] papers;
    public GameObject pressC;
    public bool isInLevel2;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //if (selectedItem != null)
        //{
        //    Debug.Log(selectedItem.name);
        //    Debug.Log(selectedItem.gameObject.tag);
        //}
        if (PlayerScript.inventory[(int)PlayerScript.selector] != null)
            selectedItem = PlayerScript.inventory[(int)PlayerScript.selector];
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction * length, Color.red);
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
            else if (hit.collider.tag == "Moveable")
            {
                UI.SetActive(true);
            }
            else if (hit.collider.tag == "Frames")
            {
                UI.SetActive(true);
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (selectedItem != null)
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
                    else
                    {
                        Debug.Log("No item selected");
                    }


                }
            }
            else if (hit.collider.tag == "lockedDoor")
            {
                UI.SetActive(true);
                if (Input.GetKeyDown(KeyCode.E))
                {
                    PuzzleUI.SetActive(true);
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    Time.timeScale = 0;
                }
            }
            else if (hit.collider.tag == "lever")
            {
                if (!leverScript) return;
                UI.SetActive(true);
                //play lever animation
                if (Input.GetKeyDown(KeyCode.E) && !leverScript.playingLeverAnimation)
                {
                    leverScript.playingLeverAnimation = true;
                    //play door animation
                    Invoke("activateCam", 3.5f);
                    Invoke("OpenDoor", 6f);
                    Invoke("disableCamera", 10.5f);
                }

            }
        }
        else
        {
            UI.SetActive(false);
        }
        if (isInLevel2)
        {
            if (selectedItem)
            {
                pressC.SetActive(true);
                if (Input.GetKeyDown(KeyCode.C) && Time.timeScale != 0)
                {
                    switch (selectedItem.name)
                    {
                        case "Book1":
                            papers[0].SetActive(true);
                            Time.timeScale = 0;
                            break;
                        case "Book2":
                            papers[1].SetActive(true);
                            Time.timeScale = 0;
                            break;
                        case "Book3":
                            papers[2].SetActive(true);
                            Time.timeScale = 0;
                            break;
                        case "Book4":
                            papers[3].SetActive(true);
                            Time.timeScale = 0;
                            break;
                        case "Book5":
                            papers[4].SetActive(true);
                            Time.timeScale = 0;
                            break;
                        default:
                            pressC.SetActive(false);
                            Time.timeScale = 0;
                            break;
                    }
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }
            }
            else
            {
                pressC.SetActive(false);
            }
        }

        /*
        if (selectedItem && isInLevel2)
        {

            if (selectedItem.name == "Book1")
            {
                pressC.SetActive(true);
                if (Input.GetKeyDown(KeyCode.C))
                {
                    papers[0].SetActive(true);
                    UnityEngine.Cursor.lockState = CursorLockMode.None;
                }
            }
            else if (selectedItem.name == "Book2")
            {
                pressC.SetActive(true);
                if (Input.GetKeyDown(KeyCode.C))
                {
                    papers[1].SetActive(true);
                    UnityEngine.Cursor.lockState = CursorLockMode.None;
                }
            }
            else if (selectedItem.name == "Book3")
            {
                pressC.SetActive(true);
                if (Input.GetKeyDown(KeyCode.C))
                {
                    papers[2].SetActive(true);
                    UnityEngine.Cursor.lockState = CursorLockMode.None;
                }
            }
            else if (selectedItem.name == "Book4")
            {
                pressC.SetActive(true);
                if (Input.GetKeyDown(KeyCode.C))
                {
                    papers[3].SetActive(true);
                    UnityEngine.Cursor.lockState = CursorLockMode.None;
                }
            }
            else if (selectedItem.name == "Book5")
            {
                pressC.SetActive(true);
                if (Input.GetKeyDown(KeyCode.C))
                {
                    papers[4].SetActive(true);
                    UnityEngine.Cursor.lockState = CursorLockMode.None;
                }
            }
            else
            {
                pressC.SetActive(false);
            }
        }
        /*
        */
    }

    private void CheckToOpenDoor()
    {
        if (numOfImgsDone >= 5)
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
