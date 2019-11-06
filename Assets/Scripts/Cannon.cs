using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{

    public float shootSpeed = 50.0f;
    public GameObject ballPrefab;
    public float firingFrequency = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Shoot();
    }

    void Shoot()
    {
        float probability = Time.deltaTime * firingFrequency;
        if(Random.value < probability)
        {
            Vector3 position = transform.position;
            
            if (transform.eulerAngles.y > 70f)
            {
                //Shoot higher position
                position.y = transform.position.y + 1f;
            }
            GameObject ball = Instantiate(ballPrefab, position, Quaternion.identity) as GameObject;            
            ball.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, -shootSpeed);
        }
    }
}
