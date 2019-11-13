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

    private bool isGrounded;

    void Start()
    {
        //controller = GetComponent<CharacterController>();
        anim = gameObject.GetComponentInChildren<Animator>();
        myRigidbody = GetComponent<Rigidbody>();
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
            Debug.Log("lock jump");
            anim.SetInteger("AnimationPar", 3);
        }
        if (!isGrounded)
        {
            IsGrounded();
        }

    }

    void IsGrounded()
    {
        isGrounded = false;
        RaycastHit[] hit;
        Debug.Log(transform.position);
        Debug.Log(isGrounded);
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
                Debug.Log("Is touching floor");
                Debug.Log(isGrounded);
            }
            else
            {
                isGrounded = false;
                Debug.Log("Is NOT touching floor");
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

        if (collision.gameObject.layer == 12)
        {
            GameObject ball = collision.gameObject;
            health -= ball.GetComponent<Ball>().damage;
            Destroy(ball);
            Debug.Log(health);
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
}
