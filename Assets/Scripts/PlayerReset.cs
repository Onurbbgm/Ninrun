using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReset : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<Player>().ResetLastCheckPoint();
    }

}
