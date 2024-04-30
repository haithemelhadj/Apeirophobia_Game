using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIArrow : MonoBehaviour
{
    public bool Incerement;
    public TextMeshProUGUI Text;

    public void Increment()
    {
        if (Incerement)
        {
            Debug.Log("Incrementing");
            Text.text = Mathf.Min((int.Parse(Text.text) + 1), 9).ToString();
        }
        else
        {
            Debug.Log("-Incrementing");
            Text.text = Mathf.Max((int.Parse(Text.text) - 1),0).ToString();
        }
    }
}
