using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinX : MonoBehaviour
{
    public float rotationSpeed = -10f;

    void FixedUpdate()
    {
        
        transform.Rotate(0, 1 * rotationSpeed,0);
    }
}
