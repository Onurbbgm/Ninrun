using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public Vector3 savedPosition = new Vector3(0f, 0f, 0f);


    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<Player>().SetLastPosition(savedPosition);
    }
}
