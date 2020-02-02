using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public Vector3 savedPosition = new Vector3(0f, 0f, 0f);


    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Player>() != null)
        {
            other.GetComponent<Player>().SetLastPosition(savedPosition);
        }
        else
        {
            other.GetComponentInParent<Player>().SetLastPosition(savedPosition);
        }
    }
}
