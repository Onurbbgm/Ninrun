using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{

    private Animator anim;
    //private CharacterController controller;
    private Rigidbody myRigidbody;

    public float speed = 600.0f;
    public float turnSpeed = 400.0f;
    private Vector3 moveDirection = Vector3.zero;
    public float gravity = 20.0f;
    public float jumpSpeed = 50.0f;
    public float health = 100.0f;
    public int fallPointLost = 100;
    public int points = 0;
    public int pointMultiplier = 2;

    private SphereCollider mySphereCollider;
    private float originalSpeed = 0f;
    private bool isGrounded;
    private Vector3 lastPosition = new Vector3(0f,0f,0f);

    private bool isInvincible = false;    
    private bool beginTimerSpeed = false;
    private bool beginTimerMagnet = false;

    private float timerSpeed = 0.0f;
    private float timerMagnet = 0.0f;
    private float timerInvincibility = 0.0f;

    private float durationSpeed = 0.0f;
    private float durationInvincibility = 0.0f;
    private float durationMagnet = 0.0f;
    

    void Start()
    {
        //controller = GetComponent<CharacterController>();
        originalSpeed = speed;
        anim = gameObject.GetComponentInChildren<Animator>();
        myRigidbody = GetComponent<Rigidbody>();
        mySphereCollider = GetComponentInChildren<SphereCollider>();
        isGrounded = true;
        anim.SetInteger("AnimationPar", 1);
    }

    //private void FixedUpdate()
    //{
    //    IsGrounded();
    //}

    void Update()
    {
        
        Movement();
        CountPointsDistance();
        if (beginTimerSpeed)
        {
            StarTimerSpeed();
        }
        if (isInvincible)
        {
            StarTimerInvincibility();
        }
        if (beginTimerMagnet)
        {
            StartTimerMagnet();
        }
        //Debug.Log(points);
        //else
        //{
        //    anim.SetInteger("AnimationPar", 0);
        //}

        //Original code
        //if (controller.isGrounded)
        //{
        //    moveDirection = transform.forward * Input.GetAxis("Vertical") * speed;
        //}

        //float turn = Input.GetAxis("Horizontal");
        //transform.Rotate(0, turn * turnSpeed * Time.deltaTime, 0);
        //controller.Move(moveDirection * Time.deltaTime);
        //moveDirection.y -= gravity * Time.deltaTime;
    }

    void Movement()
    {
        float movement = Input.GetAxis("Horizontal");
        Vector3 playerVelocity = new Vector3(movement*speed, myRigidbody.velocity.y, speed);
        //Debug.Log(playerVelocity);
        myRigidbody.velocity = playerVelocity;
        //moveDirection.y -= gravity * Time.deltaTime;
        
        if (Input.GetKeyDown("space") && isGrounded)
        {
            Vector3 jumpVelocity = new Vector3(0f, jumpSpeed, 0f);
            myRigidbody.velocity += jumpVelocity;
            isGrounded = false;
            //Debug.Log("lock jump");
            anim.SetInteger("AnimationPar", 3);
        }
        if (!isGrounded)
        {
            IsGrounded();
        }

    }

    void CountPointsDistance()
    {
        points += Mathf.RoundToInt(Time.deltaTime*60)*pointMultiplier;
    }

    void IsGrounded()
    {
        isGrounded = false;
        RaycastHit[] hit;
        //Debug.Log(transform.position);
        //Debug.Log(isGrounded);
        hit = Physics.RaycastAll(transform.position, Vector3.down, 0.01f);
        // you can increase RaycastLength and adjust direction for your case
        foreach (var hited in hit)
        {
            if (hited.collider.gameObject == gameObject) //Ignore my character
                continue;
            // Don't forget to add tag to your ground
            if (hited.collider.gameObject.layer == 8)
            { //Change it to match ground tag
                isGrounded = true;
                anim.SetInteger("AnimationPar", 1);
                //Debug.Log("Is touching floor");
                //Debug.Log(isGrounded);
            }
            else
            {
                isGrounded = false;
                //Debug.Log("Is NOT touching floor");
            }
                
            
            //Debug.DrawLine(transform.position, hited.point, Color.red);
        }
        
    }        

    private void OnCollisionEnter(Collision collision)
    {
        //if (collision.gameObject.layer == 8 && !isGrounded)
        //{
        //    isGrounded = true;
        //    Debug.Log("Is touching floor");
        //    anim.SetInteger("AnimationPar", 1);
        //}

        //if (collision.gameObject.layer == 12)
        //{
            
        //    GameObject ball = collision.gameObject;
            
        //    if (points - ball.GetComponent<Ball>().damage > 0)
        //    {
        //        points -= ball.GetComponent<Ball>().damage;
        //    }
        //    else
        //    {
        //        points = 0;
        //    }
        //    Destroy(ball);
        //    //Debug.Log(health);
        //}
    }

    private void OnTriggerEnter(Collider other)
    {        
        if(other.gameObject.layer == 17 && !isInvincible)
        {
            GameObject ball = other.gameObject;
            if (points - ball.GetComponentInParent<Ball>().damage > 0)
            {
                points -= ball.GetComponentInParent<Ball>().damage;
            }
            else
            {
                points = 0;
            }
            Destroy(ball.transform.parent.gameObject);
        }

        if (other.tag == "InvincibilityPowerUp")
        {
            if (!isInvincible)
            {
                isInvincible = true;
                timerInvincibility = 0.0f;
                durationInvincibility = other.GetComponent<PowerUps>().bonusInvincibilityDuration;
                Debug.Log("Is Invincible");
                Destroy(other.gameObject);
            }
        }

        if(other.tag == "SpeedPowerUp")
        {
            if (!beginTimerSpeed)
            {
                beginTimerSpeed = true;
                timerSpeed = 0.0f;
                Debug.Log(speed);
                originalSpeed = speed;
                speed += other.GetComponent<PowerUps>().bonusSpeed;
                Debug.Log(speed);
                durationSpeed = other.GetComponent<PowerUps>().bonusSpeedDuration;
                Destroy(other.gameObject);
            }
        }

        if(other.tag == "MagnetPowerUp")
        {
            if (!beginTimerMagnet)
            {
                beginTimerMagnet = true;
                timerMagnet = 0.0f;
                durationMagnet = other.GetComponent<PowerUps>().bonusMagnetDuration;
                mySphereCollider.radius = other.GetComponent<PowerUps>().bonusMagnetRadius;
                Debug.Log("Magenet started");
                Destroy(other.gameObject);
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        //if (collision.gameObject.layer == 8 && isGrounded)
        //{
        //    isGrounded = false;
        //    Debug.Log("Is NOT touching floor");
        //}
    }

    private void StarTimerSpeed()
    {
        timerSpeed += Time.deltaTime;
        float seconds = timerSpeed % 60f;
        if(seconds >= durationSpeed)
        {
            beginTimerSpeed = false;
            timerSpeed = 0.0f;
            speed = originalSpeed;
            Debug.Log(speed);
            Debug.Log("Time is up extra speed");
        }
    }

    private void StarTimerInvincibility()
    {
        timerInvincibility += Time.deltaTime;
        float seconds = timerInvincibility % 60f;
        if (seconds >= durationInvincibility)
        {
            isInvincible = false;
            timerInvincibility = 0.0f;            
            Debug.Log("Time is up invincibility");
        }
    }

    private void StartTimerMagnet()
    {
        timerMagnet += Time.deltaTime;
        float seconds = timerMagnet % 60f;
        if (seconds >= durationMagnet)
        {
            beginTimerMagnet = false;
            timerMagnet = 0.0f;
            mySphereCollider.radius = 0f;
            Debug.Log("Time is up magnet");
        }
    }

    public void SetLastPosition(Vector3 newPosition)
    {
        lastPosition = newPosition;
    }

    public void ResetLastCheckPoint()
    {                
        if(points-fallPointLost > 0)
        {
            points -= fallPointLost;
        }
        else
        {
            points = 0;
        }

        isInvincible = false;
        beginTimerSpeed = false;
        beginTimerMagnet = false;
        speed = originalSpeed;
        mySphereCollider.radius = 0f;
        timerSpeed = 0.0f;
        timerInvincibility = 0.0f;
        timerMagnet = 0.0f;
        var positions = FindObjectsOfType<Follow>();
        float[] distances = new float[positions.Length];
        int count = 0;
        foreach(var pos in positions)
        {
            distances[count] = Mathf.Abs(Mathf.Abs(pos.transform.position.z) - Mathf.Abs(transform.position.z));
            count++;
        }

        transform.position = lastPosition;
        count = 0;
        foreach (var pos in positions)
        {
            float z = transform.position.z + distances[count];
            pos.transform.position = new Vector3(pos.transform.position.x, pos.transform.position.y, z);
            count++;
        }
        var cannon = FindObjectOfType<Cannon>();
        cannon.transform.position = positions[1].transform.position;
        cannon.ResetCurrentPosition();
    }

}
