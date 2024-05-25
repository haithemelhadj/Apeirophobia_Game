using UnityEngine;

public class CloseUI : MonoBehaviour
{
    public GameObject UI;
    public void Close()
    {
        UI.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
