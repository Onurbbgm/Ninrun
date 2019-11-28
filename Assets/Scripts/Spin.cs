using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    public float rotationSpeed = 200f;
    
    void FixedUpdate()
    {
        transform.Rotate(Vector3.up * 200f * Time.fixedDeltaTime);
    }
}
