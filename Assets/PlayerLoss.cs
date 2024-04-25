using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLoss : MonoBehaviour
{
    public static void Loss()
    {
        //reload same active scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
