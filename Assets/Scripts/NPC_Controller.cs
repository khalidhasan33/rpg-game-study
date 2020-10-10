using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Controller : MonoBehaviour
{
    private Animator animator;
    public float speed;
    private float waitTime;
    public float startWaitTime;
    public bool move;
    private bool isMoving = true;
    private DialogueTrigger dialogueTrigger;
    public DialogueManager dialogueManager;

    public Transform[] moveSpots;
    private int randomSpot;

    private float lastX;
    private float lastY;
    private bool onDialog = false;
    private bool firstSentence = true;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        dialogueTrigger = GetComponent<DialogueTrigger>();
        waitTime = startWaitTime;
        if (move)
        {
            randomSpot = Random.Range(0, moveSpots.Length);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving && move && !onDialog)
        {
            float step = speed * Time.deltaTime;
            Vector3 directionToTarget = moveSpots[randomSpot].position - transform.position;

            if (Mathf.Abs(directionToTarget.x) < Mathf.Abs(directionToTarget.y) && Mathf.Abs(directionToTarget.x) > Mathf.Epsilon)
            {
                lastY = directionToTarget.y;
                directionToTarget.y = 0f;
            }
            else if (Mathf.Abs(directionToTarget.y) > Mathf.Epsilon)
            {
                lastX = directionToTarget.x;
                directionToTarget.x = 0f;
            }

            animator.SetFloat("moveX", directionToTarget.x);
            animator.SetFloat("moveY", directionToTarget.y);

            transform.position = Vector3.MoveTowards(
                transform.position,
                transform.position + directionToTarget,
                step
            );

            if (moveSpots[randomSpot].position == transform.position)
            {
                animator.SetBool("moving", false);
                animator.SetFloat("moveX", lastX);
                animator.SetFloat("moveY", lastY);
            }
            else
            {
                animator.SetBool("moving", true);
            }

            if (Vector3.Distance(transform.position, moveSpots[randomSpot].position) < 0.2f)
            {
                if (waitTime <= 0)
                {
                    randomSpot = Random.Range(0, moveSpots.Length);
                    waitTime = startWaitTime;
                }
                else
                {
                    waitTime -= Time.deltaTime;
                }
            }
        }
        if (onDialog)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                if (firstSentence)
                {
                    dialogueTrigger.TriggerDialogue();
                    firstSentence = false;
                }
                else
                {
                    dialogueManager.DisplayNextSentence();
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            isMoving = false;
            animator.SetBool("moving", false);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            isMoving = false;
            animator.SetBool("moving", false);
            if (Input.GetKeyDown(KeyCode.Z))
            {
                //dialogueTrigger.TriggerDialogue();
                Debug.Log("First");
                onDialog = true;
                //dialogueManager.DisplayNextSentence();
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player" && move)
        {
            isMoving = true;
            animator.SetBool("moving", true);
        }
        if (collision.collider.tag == "Player")
        {
            onDialog = false;
            firstSentence = true;
        }
    }
}
