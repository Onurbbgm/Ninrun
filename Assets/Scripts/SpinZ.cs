using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinZ : MonoBehaviour
{
    public float rotationSpeed = 200f;

    void FixedUpdate()
    {       
        transform.Rotate(0,0,1*rotationSpeed);
    }

}
