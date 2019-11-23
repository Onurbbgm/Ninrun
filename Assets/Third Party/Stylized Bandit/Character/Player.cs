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
    public float volumeSoundEffects = 1f;
    public AudioClip hitSound;    
    public AudioClip restartSound;
    public AudioClip powerUpSound;
    public AudioClip jumpSound;

    private SphereCollider mySphereCollider;
    private AudioSource myAudioSource;
    private float originalSpeed = 0f;
    private bool isGrounded;
    private Vector3 lastPosition = new Vector3(0f,0f,0f);

    private bool isInvincible = false;    
    private bool beginTimerSpeed = false;
    private bool beginTimerMagnet = false;
    private bool levelFinished = false;
    private bool checkGround = false;
    private bool countTimeJump = false; 

    private Coroutine speedTimer = null;
    private Coroutine invincibilityTimer = null;
    private Coroutine magnetTimer = null;

    private float durationSpeed = 0.0f;
    private float durationInvincibility = 0.0f;
    private float durationMagnet = 0.0f;
    private float nextJumpTime = 0.2f;

    void Start()
    {
        //controller = GetComponent<CharacterController>();
        originalSpeed = speed;
        anim = gameObject.GetComponentInChildren<Animator>();
        myRigidbody = GetComponent<Rigidbody>();
        mySphereCollider = GetComponentInChildren<SphereCollider>();
        isGrounded = true;
        anim.SetInteger("AnimationPar", 1);
        myAudioSource = GetComponent<AudioSource>();
        levelFinished = false;
    }

    //private void FixedUpdate()
    //{
    //    IsGrounded();
    //}

    void Update()
    {
        
        Movement();
        CountPointsDistance();
        //if (beginTimerSpeed)
        //{
        //    StartTimerSpeed();
        //}
        //if (isInvincible)
        //{
        //    StartTimerInvincibility();
        //}
        //if (beginTimerMagnet)
        //{
        //    StartTimerMagnet();
        //}
        //if (countTimeJump)
        //{
        //    StartTimerNextJump();
        //}
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
            //Debug.Log("floor:"+isGrounded);
            anim.SetInteger("AnimationPar", 3);
            myAudioSource.Stop();
            AudioSource.PlayClipAtPoint(jumpSound,transform.position, volumeSoundEffects);
            countTimeJump = true;
            nextJumpTime = 0.0f;
            StartCoroutine(StartTimerNextJump());
        }
        else if (isGrounded && !myAudioSource.isPlaying && !levelFinished)
        {
            myAudioSource.Play();
        }        
        if (!isGrounded && checkGround)
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
        //isGrounded = false;
        //RaycastHit[] hit;
        RaycastHit hit;
        int mask = 1 << LayerMask.NameToLayer("Floor");
        //Debug.Log(transform.position);
        //Debug.Log(isGrounded);
        //hit = Physics.RaycastAll(transform.position, Vector3.down, 0.01f);
        
        //Debug.Log("Layer: "+mask);
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 0.01f, mask))
        {
            //Debug.Log("Is touching floor");
            isGrounded = true;
            checkGround = false;
            //Debug.Log("floot:" + isGrounded);
            anim.SetInteger("AnimationPar", 1);
            if (!myAudioSource.isPlaying)
            {
                myAudioSource.Play();
            }            
        }
       
        // you can increase RaycastLength and adjust direction for your case
        //foreach (var hited in hit)
        //{
        //    if (hited.collider.gameObject == gameObject) //Ignore my character
        //        continue;
        //    // Don't forget to add tag to your ground
        //    if (hited.collider.gameObject.layer == 8)
        //    { //Change it to match ground tag
        //        isGrounded = true;
        //        anim.SetInteger("AnimationPar", 1);
        //        myAudioSource.Play();                
        //        //Debug.Log("Is touching floor");
        //        //Debug.Log(isGrounded);
        //    }
        //    else
        //    {
        //        isGrounded = false;
        //        //Debug.Log("Is NOT touching floor");
        //    }
                
            
        //    //Debug.DrawLine(transform.position, hited.point, Color.red);
        //}
        
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
            AudioSource.PlayClipAtPoint(hitSound, transform.position, volumeSoundEffects);
            Destroy(ball.transform.parent.gameObject);
        }

        if (other.tag == "InvincibilityPowerUp")
        {
            if (!isInvincible)
            {
                isInvincible = true;                
                durationInvincibility = other.GetComponent<PowerUps>().bonusInvincibilityDuration;
                //Debug.Log("Is Invincible");
                invincibilityTimer = StartCoroutine(StartTimerInvincibility());
                Destroy(other.gameObject);
            }
            AudioSource.PlayClipAtPoint(powerUpSound, transform.position, volumeSoundEffects);
        }

        if(other.tag == "SpeedPowerUp")
        {
            if (!beginTimerSpeed)
            {
                beginTimerSpeed = true;
                //Debug.Log(speed);
                originalSpeed = speed;
                speed += other.GetComponent<PowerUps>().bonusSpeed;
                //Debug.Log(speed);
                durationSpeed = other.GetComponent<PowerUps>().bonusSpeedDuration;
                speedTimer = StartCoroutine(StartTimerSpeed());
                Destroy(other.gameObject);
                AudioSource.PlayClipAtPoint(powerUpSound, transform.position, volumeSoundEffects);
            }
        }

        if(other.tag == "MagnetPowerUp")
        {
            if (!beginTimerMagnet)
            {
                beginTimerMagnet = true;
                durationMagnet = other.GetComponent<PowerUps>().bonusMagnetDuration;
                mySphereCollider.radius = other.GetComponent<PowerUps>().bonusMagnetRadius;
                //Debug.Log("Magenet started");
                magnetTimer = StartCoroutine(StartTimerMagnet());
                Destroy(other.gameObject);
                AudioSource.PlayClipAtPoint(powerUpSound, transform.position, volumeSoundEffects);
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

    private IEnumerator StartTimerNextJump()
    {
        yield return new WaitForSecondsRealtime(nextJumpTime);      
        checkGround = true;
        countTimeJump = false;
        //nextJumpTime += Time.deltaTime;
        ////Debug.Log(nextJumpTime);
        //float seconds = timerSpeed % 60f;
        //Debug.Log("seconds "+seconds+" nextJumpTime "+nextJumpTime);
        //if (seconds >= 0.5f)
        //{
        //    nextJumpTime = 0.0f;
        //    checkGround = true;
        //    countTimeJump = false;
        //    Debug.Log("countTimeJump:"+countTimeJump);
        //}
        
    }

    private IEnumerator StartTimerSpeed()
    {
        Debug.Log("start timer speed");
        //timerSpeed += Time.deltaTime;
        //float seconds = timerSpeed % 60f;
        yield return new WaitForSecondsRealtime(durationSpeed);
        Debug.Log("end timer speed");
        beginTimerSpeed = false;
        speed = originalSpeed;
        //if(seconds >= durationSpeed)
        //{
        //    beginTimerSpeed = false;
        //    timerSpeed = 0.0f;
        //    speed = originalSpeed;
        //    //Debug.Log(speed);
        //    //Debug.Log("Time is up extra speed");
        //}
    }

    private IEnumerator StartTimerInvincibility()
    {
        Debug.Log("start timer invi");
        yield return new WaitForSecondsRealtime(durationInvincibility);
        isInvincible = false;
        Debug.Log("stop timer invi");
        //timerInvincibility += Time.deltaTime;
        //float seconds = timerInvincibility % 60f;
        //if (seconds >= durationInvincibility)
        ////{
        //    isInvincible = false;
        //    timerInvincibility = 0.0f;            
        //    //Debug.Log("Time is up invincibility");
        //}
    }

    private IEnumerator StartTimerMagnet()
    {
        Debug.Log("start timer mag");
        yield return new WaitForSecondsRealtime(durationMagnet);
        Debug.Log("stop timer magn");
        beginTimerMagnet = false;
        mySphereCollider.radius = 0f;
        //timerMagnet += Time.deltaTime;
        //float seconds = timerMagnet % 60f;
        //if (seconds >= durationMagnet)
        //{
        //    beginTimerMagnet = false;
        //    timerMagnet = 0.0f;
        //    mySphereCollider.radius = 0f;
        //    //Debug.Log("Time is up magnet");
        //}
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
        myAudioSource.Stop();
        isInvincible = false;
        beginTimerSpeed = false;
        beginTimerMagnet = false;
        isGrounded = true;
        checkGround = false;
        countTimeJump = false;
        //nextJumpTime = 0.3f;
        speed = originalSpeed;
        mySphereCollider.radius = 0f;        
        StopCoroutine(speedTimer);
        StopCoroutine(invincibilityTimer);
        StopCoroutine(magnetTimer);
        var positions = FindObjectsOfType<Follow>();
        float[] distances = new float[positions.Length];
        int count = 0;
        foreach(var pos in positions)
        {
            distances[count] = Mathf.Abs(Mathf.Abs(pos.transform.position.z) - Mathf.Abs(transform.position.z));
            count++;
        }

        transform.position = lastPosition;
        AudioSource.PlayClipAtPoint(restartSound, transform.position, volumeSoundEffects);
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

    public void FinishLevel()
    {
        myAudioSource.Stop();
        levelFinished = true;
    }

}
