using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{

    public float shootSpeed = 50.0f;
    public GameObject ballPrefab;
    public float firingFrequency = 0.5f;
    public GameObject[] positions;

    private Rigidbody myRigidbody;
    private int currentPos = 0;
    private bool done = false;
    private float smothSpeed = 20f;

    private GameObject pausePanel;

    // Start is called before the first frame update
    void Start()
    {
        pausePanel = GameObject.FindGameObjectWithTag("Pause Screen");
        if(pausePanel == null)
        {
            pausePanel = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().pausePanel;
        }
        myRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Shoot();
        Move();
    }

    void Shoot()
    {
        float probability = Time.deltaTime * firingFrequency;
        if (Random.value < probability)
        {
            Vector3 position = transform.GetChild(0).transform.position;           
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            GameObject ball = Instantiate(ballPrefab, position, Quaternion.identity) as GameObject;            
            Vector3 ballVelocity = (player.transform.position - ball.transform.position).normalized * shootSpeed;
            ball.GetComponent<Rigidbody>().velocity = ballVelocity;
        }
    }

    void Move()
    {       
        if (pausePanel.activeInHierarchy)
        {
            return;
        }
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector3 cannonVelocity = new Vector3(myRigidbody.velocity.x, myRigidbody.velocity.y, player.GetComponent<Player>().speed);
        //Debug.Log(cannonVelocity);
        myRigidbody.velocity = cannonVelocity;
        transform.LookAt(player.transform);
        

        if (!done)
        {
            //Goes forward
            for (int i = currentPos; i < positions.Length; i++)
            {

                if (Mathf.Abs(transform.position.x - positions[i].transform.position.x) > 0.1)
                {
                    GoTo(transform.position, positions[i].transform.position, smothSpeed);

                    break;
                }
                else if (currentPos != positions.Length - 1)
                {
                    currentPos++;
                    smothSpeed += 40f;                   
                    //Debug.Log("Current pos " + currentPos);
                }
                else
                {
                    currentPos = positions.Length - 1;
                    smothSpeed += 40f;
                    done = true;
                    break;
                }
                //yield return StartCoroutine(GoTo(transform, pointB, pointA, 3.0f));
            }
        }
        else
        {
            //Goes bacwards
            for (int i = currentPos; i >= 0 ; i--)
            {

                if (Mathf.Abs(transform.position.x - positions[i].transform.position.x) > 0.1)
                {
                    GoTo(transform.position, positions[i].transform.position, smothSpeed);

                    break;
                }
                else if (currentPos != 0)
                {
                    currentPos--;
                    smothSpeed += 40f;
                    //Debug.Log("Current pos " + currentPos);
                }
                else
                {
                    currentPos = 0;
                    smothSpeed += 40f;
                    done = false;
                    break;
                }
                //yield return StartCoroutine(GoTo(transform, pointB, pointA, 3.0f));
            }
        }

       
    }

    void GoTo(Vector3 pointA, Vector3 pointB, float time)
    {
        transform.position = Vector3.Lerp(pointA, pointB, Mathf.SmoothStep(0f, 1f,Mathf.PingPong(Time.time / time, 1f)));
        //var i = 0.0f;
        //var rate = 1.0f / time;
        //Debug.Log("Ate aqui");
        //while (i < 1.0f)
        //{
        //    Debug.Log(pointB);
        //    i += Time.deltaTime * rate;
        //    transform.position = Vector3.Lerp(pointA, pointB, i);
        //    yield return null;
        //}
    }

    public void ResetCurrentPosition()
    {
        currentPos = 0;
    }

}
