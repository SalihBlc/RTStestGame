using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeMenuController : MonoBehaviour
{
    public GameObject ESCMenu;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            ESCMenu.SetActive(!ESCMenu.activeSelf);
    }
}
