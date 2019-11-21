using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointPickUp : MonoBehaviour
{
    public int pointValue = 100;

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
        if (other.name == "Player")
        {
            other.GetComponent<Player>().points += pointValue;
        }
        else
        {
            other.GetComponentInParent<Player>().points += pointValue;
        }
        
    }

}
