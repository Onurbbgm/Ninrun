using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReset : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {        
        if (other.GetComponent<Player>() != null)
        {
            other.GetComponent<Player>().ResetLastCheckPoint();
        }
        else
        {
            other.GetComponentInParent<Player>().ResetLastCheckPoint();
        }
    }

}
