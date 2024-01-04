using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Controller : MonoBehaviour
{
    private Animator animator;
    private DialogueTrigger dialogueTrigger;
    public DialogueManager dialogueManager;
    public PlayerMovement playerMovement;

    public Transform[] moveSpots;
    private int randomSpot;
    public bool move;
    public float speed;
    public float startWaitTime;
    private float waitTime;

    private float lastX;
    private float lastY;
    private bool onDialog = false;
    private bool onDialogOnRange = false;
    private bool isMoving = true;
    private float nextDialogueDelay = 0.5f;
    private float nextDialogue = 0.15f;

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
        if (onDialogOnRange)
        {
            if (Input.GetKeyDown(KeyCode.Z) && Time.time > nextDialogue)
            {
                animator.SetFloat("moveX", -playerMovement.lastX);
                animator.SetFloat("moveY", -playerMovement.lastY);
                if (!dialogueManager.onDialogue)
                {
                    dialogueTrigger.TriggerDialogue();
                }
                else
                {
                    dialogueManager.DisplayNextSentence();
                }
                nextDialogue = Time.time + nextDialogueDelay;
            }
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            isMoving = false;
            animator.SetBool("moving", false);
            onDialogOnRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            dialogueManager.EndDialogue();
            onDialogOnRange = false;
        }
        if (other.gameObject.tag == "Player" && move)
        {
            isMoving = true;
            animator.SetBool("moving", true);
        }
    }

}
