using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float runningSpeed = 8f;
    private Rigidbody2D myRigidbody;
    private Vector2  movement;
    private Animator animator;

    private float timeDownX = 0.0f; //time which horizontal was pressed
    private float timeDownY = 0.0f; //time which vertical was pressed

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        movement = Vector2.zero;
        float xVel = Input.GetAxisRaw("Horizontal");
        float yVel = Input.GetAxisRaw("Vertical");

        //if key is pressed on Horizontal save time it was pressed
        if (xVel != 0)
        {
            if (timeDownX == 0.0f) //if we don't have a stored time for it
                timeDownX = Time.time;
        }
        else
        {
            timeDownX = 0.0f; // reset time if no button is being pressed
        }

        //if key is pressed on vertical save time it was pressed
        if (yVel != 0)
        {
            if (timeDownY == 0.0f) //if we don't have a stored time for it
                timeDownY = Time.time;
        }
        else
        {
            timeDownY = 0.0f; // reset time if no button is being pressed
        }

        if (timeDownX > timeDownY)
        {
            movement = Vector2.right * xVel;
        }
        else if (timeDownX < timeDownY)
        {
            movement = Vector2.up * yVel;
        }

        if (movement != Vector2.zero)
        {
            animator.SetFloat("moveX", movement.x);
            animator.SetFloat("moveY", movement.y);
            animator.SetBool("moving", true);
        }
        else
        {
            animator.SetBool("moving", false);
            animator.SetBool("running", false);
        }
    }

    void FixedUpdate()
    {
        bool isRunning = Input.GetKey(KeyCode.X);
        if (isRunning)
        {
            myRigidbody.MovePosition(myRigidbody.position + movement * runningSpeed * Time.fixedDeltaTime);
            animator.SetBool("running", true);
        }
        else
        {
            myRigidbody.MovePosition(myRigidbody.position + movement * moveSpeed * Time.fixedDeltaTime);
            animator.SetBool("running", false);
        }
    }
}
