using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public float damage = 10f;

    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        //var  layerX = player.transform.position.x;
       // transform.position = new Vector3(player.transform.position.x, transform.position.y, transform.position.z);
       if(transform.position.y < -1f)
        {
            Destroy(gameObject);
        }
    }

   

    
}
