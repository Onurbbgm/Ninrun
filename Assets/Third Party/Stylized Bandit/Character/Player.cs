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
            anim.SetInteger("AnimationPar", 3);
        }
      
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == 8 && !isGrounded)
        {
            isGrounded = true;
            anim.SetInteger("AnimationPar", 1);
        }

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
        if (collision.gameObject.layer == 8 && isGrounded)
        {
            isGrounded = false;
        }
    }
}
