using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public DialogueManager dialogueManager;
    private Rigidbody2D myRigidbody;
    private Vector2 movement;
    private Animator animator;

    public float moveSpeed = 5f;
    public float runningSpeed = 8f;
    public float lastX;
    public float lastY;

    private float timeDownX = 0.0f;
    private float timeDownY = 0.0f;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!dialogueManager.onDialogue)
        {
            movement = Vector2.zero;
            float xVel = Input.GetAxisRaw("Horizontal");
            float yVel = Input.GetAxisRaw("Vertical");
            if (xVel != 0)
            {
                if (timeDownX == 0.0f)
                    timeDownX = Time.time;
            }
            else
            {
                timeDownX = 0.0f;
            }
            if (yVel != 0)
            {
                if (timeDownY == 0.0f)
                    timeDownY = Time.time;
            }
            else
            {
                timeDownY = 0.0f;
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
                lastX = movement.x;
                animator.SetFloat("moveY", movement.y);
                lastY = movement.y;
                animator.SetBool("moving", true);
            }
            else
            {
                animator.SetBool("moving", false);
                animator.SetBool("running", false);
            }
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
