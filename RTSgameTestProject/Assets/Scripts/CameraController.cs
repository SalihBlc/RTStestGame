using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    void Start()
    {

    }

    void FixedUpdate()
    {
        this.transform.Translate(Input.GetAxis("Horizontal"), 0, 0);
        this.transform.Translate(0, Input.GetAxis("Vertical"), 0);
    }
}
