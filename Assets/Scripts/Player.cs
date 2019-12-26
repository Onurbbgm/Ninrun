using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{

    private Animator anim;
    //private CharacterController controller;
    private Rigidbody myRigidbody;

    public float speed = 7.0f;
    public float turnSpeed = 400.0f;
    private Vector3 moveDirection = Vector3.zero;
    public float gravity = 100.0f;
    public float jumpSpeed = 4.0f;
    public float health = 100.0f;
    public int fallPointLost = 500;
    public int points = 0;
    public int pointMultiplier = 2;
    public float volumeSoundEffects = 1f;
    public float jumpMax = 7.0f;
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
    private bool addJump = true;
    private bool firstJump = true;

    private Coroutine speedTimer = null;
    private Coroutine invincibilityTimer = null;
    private Coroutine magnetTimer = null;
    private Coroutine nextJump = null;

    private float durationSpeed = 0.0f;
    private float durationInvincibility = 0.0f;
    private float durationMagnet = 0.0f;
    private float nextJumpTime = 0.2f;
    private float addJumpTime = 0.4f;
    private ParticleSystem magnetParticle;

    public GameObject pausePanel;    

    void Start()
    {
        Time.timeScale = 1;
        magnetParticle = GetComponent<ParticleSystem>();
        magnetParticle.Stop();
        pausePanel = GameObject.FindGameObjectWithTag("Pause Screen");
        pausePanel.SetActive(false);
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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!pausePanel.activeInHierarchy)
            {
                PauseGame();
            }
            else if (pausePanel.activeInHierarchy)
            {
                ContinueGame();
            }
        }       
    }

    void PauseGame()
    {
        Time.timeScale = 0;
        pausePanel.SetActive(true);
        myAudioSource.Stop();
        Debug.Log("Paused");
        
    }

    void ContinueGame()
    {
        Time.timeScale = 1;
        pausePanel.SetActive(false);
        myAudioSource.Play();
        Debug.Log("Continued");
    }

    void Movement()
    {
        //float movement = Input.GetAxis("Horizontal");
        //Vector3 playerVelocity = new Vector3(movement*speed, myRigidbody.velocity.y, speed);
        //Debug.Log(playerVelocity);
        //myRigidbody.velocity = playerVelocity;
        if (Input.GetAxis("Mouse X") < 0)
        {
            Vector3 playerVelocity = new Vector3(Input.GetAxis("Mouse X") * speed, myRigidbody.velocity.y, speed);
            //Debug.Log(Input.GetAxis("Mouse X"));
            myRigidbody.velocity = playerVelocity;
        }
        if(Input.GetAxis("Mouse X") > 0)
        {
            Vector3 playerVelocity = new Vector3(Input.GetAxis("Mouse X") * speed, myRigidbody.velocity.y, speed);
            //Debug.Log(Input.GetAxis("Mouse X"));
            myRigidbody.velocity = playerVelocity;
        } 
            //moveDirection.y -= gravity * Time.deltaTime;

        if (Input.GetMouseButton(0) && jumpSpeed < jumpMax && addJump)//&& isGrounded)
        {
            Vector3 jumpVelocity = new Vector3(0f, jumpSpeed, 0f);
            myRigidbody.velocity += jumpVelocity;
            jumpSpeed += 1.0f;
            Debug.Log(jumpSpeed);
            addJump = false;
            isGrounded = false;
            //Debug.Log("floor:"+isGrounded);
            if (firstJump)
            {
                anim.SetTrigger("Jump");
                myAudioSource.Stop();
                AudioSource.PlayClipAtPoint(jumpSound, transform.position, volumeSoundEffects);
                firstJump = false;
            }                       
            countTimeJump = true;
            nextJumpTime = 0.0f;
            nextJump = StartCoroutine(StartTimerNextJump());
            StartCoroutine(StartTimerAddJump());

        }
        else if (isGrounded && !myAudioSource.isPlaying && !levelFinished && !pausePanel.activeInHierarchy)
        {
            myAudioSource.Play();
        }        
        if (!isGrounded && checkGround)
        {            
            IsGrounded();
            //Debug.Log("NOT grounded");
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
        if (Physics.Raycast(transform.position + Vector3.up * 0.01f, Vector3.down, out hit, 0.01f, mask))
        {
            //Debug.Log("Is touching floor RAYCAST");
            isGrounded = true;
            checkGround = false;
            jumpSpeed = 4f;
            addJump = true;
            firstJump = true;
            //Debug.Log("is grounded");
            //anim.SetInteger("AnimationPar", 1);
            if (!myAudioSource.isPlaying)
            {
                myAudioSource.Play();
            }
        }
        
        Debug.DrawLine(transform.position, hit.point, Color.red);
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

    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Floor"))
        {
            //Debug.Log("is NOT on the ground");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Floor"))
        {
            //Debug.Log("IS on the ground");
            isGrounded = true;
            jumpSpeed = 4f;
            addJump = true;
            firstJump = true;
            if (nextJump != null)
            {
                StopCoroutine(nextJump);
                checkGround = false;
                //Debug.Log("STOPPED");
            }
        }
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
            }
            Destroy(other.gameObject);
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
                myAudioSource.pitch = 0.89f;
                //Debug.Log(speed);
                durationSpeed = other.GetComponent<PowerUps>().bonusSpeedDuration;
                speedTimer = StartCoroutine(StartTimerSpeed());                
            }
            Destroy(other.gameObject);
            AudioSource.PlayClipAtPoint(powerUpSound, transform.position, volumeSoundEffects);
        }

        if(other.tag == "MagnetPowerUp")
        {
            if (!beginTimerMagnet)
            {
                beginTimerMagnet = true;
                durationMagnet = other.GetComponent<PowerUps>().bonusMagnetDuration;
                mySphereCollider.radius = other.GetComponent<PowerUps>().bonusMagnetRadius;
                //Debug.Log("Magenet started");
                magnetParticle.Play();
                magnetTimer = StartCoroutine(StartTimerMagnet());                
            }
            Destroy(other.gameObject);
            AudioSource.PlayClipAtPoint(powerUpSound, transform.position, volumeSoundEffects);
        }
    }
    
    private IEnumerator StartTimerNextJump()
    {
        while (pausePanel.activeInHierarchy)
        {
            yield return null;
        }
        yield return new WaitForSeconds(nextJumpTime);      
        checkGround = true;
        countTimeJump = false;        
        
    }

    private IEnumerator StartTimerSpeed()
    {
        
        //Debug.Log("start timer speed");
        //timerSpeed += Time.deltaTime;
        //float seconds = timerSpeed % 60f;
        yield return new WaitForSeconds(durationSpeed);
        //Debug.Log("end timer speed");
        beginTimerSpeed = false;
        speed = originalSpeed;
        myAudioSource.pitch = 0.80f;
       
    }

    private IEnumerator StartTimerInvincibility()
    {
        
        //Debug.Log("start timer invi");
        yield return new WaitForSeconds(durationInvincibility);
        isInvincible = false;
        //Debug.Log("stop timer invi");
      
    }

    private IEnumerator StartTimerMagnet()
    {
        //Debug.Log("start timer mag");
        yield return new WaitForSeconds(durationMagnet);
        //Debug.Log("stop timer magn");
        beginTimerMagnet = false;
        mySphereCollider.radius = 0f;
        magnetParticle.Stop();      
    }

    private IEnumerator StartTimerAddJump()
    {
        yield return new WaitForSeconds(addJumpTime);
        Debug.Log(addJumpTime);
        addJump = true;
        
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
        myAudioSource.pitch = 0.80f;
        if(speedTimer != null)
        {
            StopCoroutine(speedTimer);
        }
        if(invincibilityTimer != null)
        {
            StopCoroutine(invincibilityTimer);
        }
        if(magnetTimer != null)
        {
            StopCoroutine(magnetTimer);
        }   
        if(nextJump != null)
        {
            StopCoroutine(nextJump);
        }
        if (magnetParticle.isPlaying)
        {            
            magnetParticle.Stop(true);
        }
        var positions = FindObjectsOfType<Follow>();
        float[] distances = new float[positions.Length];
        int count = 0;
        foreach(var pos in positions)
        {
            distances[count] = Mathf.Abs(pos.transform.position.z - transform.position.z);          
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
        if (cannon)
        {
            cannon.transform.position = positions[1].transform.position;
            cannon.ResetCurrentPosition();
        }
        
    }

    public void FinishLevel()
    {
        myAudioSource.Stop();
        levelFinished = true;
    }

}
