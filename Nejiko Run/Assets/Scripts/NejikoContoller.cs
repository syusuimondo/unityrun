using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NejikoContoller : MonoBehaviour
{
    const int MinLane = -2;
    const int MaxLane = 2;
    const float LaneWidth = 1.0f;
    const int DefaultLife = 3;
    const float StunDuration = 0.5f;
    CharacterController controller;
    Animator animator;

    Vector3 moveDirection = Vector3.zero;
    int targetLane;
    int life = DefaultLife;
    float recoverTime = 0.0f;

    public float gravity;
    public float speedz;
    public float speedx;
    public float speedJump;
    public float accelerationz;
    public int Life()
    {
        return life;
    }
    bool IsStun()
    {
        return recoverTime > 0.0f || life <= 0;
    }
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("left")) MoveToTopLeft();
        if (Input.GetKeyDown("right")) MoveToTopRight();
        if (Input.GetKeyDown("space")) Jump();
        if (IsStun())
        {
            moveDirection.x = 0.0f;
            moveDirection.z = 0.0f;
            recoverTime -= Time.deltaTime;
        }
        else
        {
            float acceleratedz = moveDirection.z + (accelerationz * Time.deltaTime);
            moveDirection.z = Mathf.Clamp(acceleratedz, 0, speedz);

            float ratiox = (targetLane * LaneWidth - transform.position.x) / LaneWidth;
            moveDirection.x = ratiox * speedx;
            moveDirection.y -= gravity * Time.deltaTime;

            Vector3 globalDirection = transform.TransformDirection(moveDirection);
            controller.Move(globalDirection * Time.deltaTime);

            if (controller.isGrounded) moveDirection.y = 0;

            animator.SetBool("run", moveDirection.z > 0.0f);
        }
    }
    public void MoveToLeft()
    {
        if (IsStun()) return;
        if (controller.isGrounded && targetLane > MinLane) targetLane--;
    }
    public void MoveToTopLeft()
    {
        if (IsStun()) return;
        if (controller.isGrounded && targetLane > MinLane) targetLane = -2;
    }
    public void MoveToRight()
    {
        if (IsStun()) return;
        if (controller.isGrounded && targetLane < MaxLane) targetLane++;
    }
    public void MoveToTopRight()
    {
        if (IsStun()) return;
        if (controller.isGrounded && targetLane < MaxLane) targetLane = 2;
    }
    public void Jump()
    {
        if (IsStun()) return;
        if (controller.isGrounded)
        {
            moveDirection.y = speedJump;
            animator.SetTrigger("jump");
        }
    }
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (IsStun()) return;
        if(hit.gameObject.tag == "Robo")
        {
            life--;
            recoverTime = StunDuration;
            animator.SetTrigger("damage");
            Destroy(hit.gameObject);
        }
    }
}
