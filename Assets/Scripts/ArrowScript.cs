using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    public Transform Red;
    public Transform Blue;
    public Transform Green;
    public Transform Yellow;

    public float RotationSpeed;

    private void Start()
    {
        transform.position = Blue.position;
    }

    void Update()
    {
        transform.Rotate(Vector3.up, RotationSpeed * Time.deltaTime);
    }

    public void SelectBlue()
    {
        transform.position = Blue.position;
    }

    public void SelectRed()
    {
        transform.position = Red.position;
    }

    public void SelectYellow()
    {
        transform.position = Yellow.position;
    }

    public void SelectGreen()
    {
        transform.position = Green.position;
    }
}
