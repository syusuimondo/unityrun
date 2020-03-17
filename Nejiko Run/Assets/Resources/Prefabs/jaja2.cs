using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jaja2 : MonoBehaviour
{
    CharacterController controller;
    Animator animator;
   
    Vector3 moveDirection;
    public float gravity;
    public float speedz;
    public float accelerationz;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        float acceleratedz = moveDirection.z + (accelerationz * Time.deltaTime);
        moveDirection.z = speedz;

        moveDirection.y -= gravity * Time.deltaTime;

        Vector3 globalDirection = transform.TransformDirection(moveDirection);
        controller.Move(globalDirection * Time.deltaTime);

        if (controller.isGrounded) moveDirection.y = 0;

        animator.SetBool("run", moveDirection.z > 0.0f);
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag == "Robo" || hit.gameObject.tag == "Item")
        {
            Destroy(hit.gameObject);
            Destroy(this.gameObject);
        }
    }
}