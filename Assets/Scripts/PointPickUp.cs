using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointPickUp : MonoBehaviour
{
    public int pointValue = 100;
    public AudioClip pickUpSound;

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
        if (other.name == "Player")
        {
            other.GetComponent<Player>().points += pointValue;
            AudioSource.PlayClipAtPoint(pickUpSound, other.gameObject.transform.position, 1f);
        }
        else if(other.name == "Magnet")
        {
            other.GetComponentInParent<Player>().points += pointValue;
            AudioSource.PlayClipAtPoint(pickUpSound, other.gameObject.transform.position, 1f);
        }
        
    }

}
