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
    AudioSource audio;

    Vector3 moveDirection = Vector3.zero;
    Vector3 globalDirection;
    int targetLane;
    int life = DefaultLife;
    int count = 0;
    float recoverTime = 0.0f;
    public GameConroller gameConroller;

    public float gravity;
    public float speedz;
    public float speedx;
    public float speedJump;
    public float accelerationz;
    public int Life()
    {
        return life;
    }
    public int Count()
    {
        return count;
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
        audio = GetComponent<AudioSource>();
        //垂直同期をオンにする。0だとオフ。
        QualitySettings.vSyncCount = 1;
        //フレームレートもついでに設定している。
        Application.targetFrameRate = 60;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("left")) MoveToLeft();
        if (Input.GetKeyDown("right")) MoveToRight();
        if (Input.GetKeyDown("w")) Jump();
        if (Input.GetKeyDown("a")) MoveToTopLeft(); ;
        if (Input.GetKeyDown("d")) MoveToTopRight(); ;
        if (Input.GetKeyDown("s")) MoveToTop();
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

            globalDirection = transform.TransformDirection(moveDirection);
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
    public void MoveToTop()
    {
        if (count > 0)
        {
            gameConroller.GetScore();
            // プレハブを取得
            GameObject prefab = (GameObject)Resources.Load("Prefabs/jaja");
            // プレハブからインスタンスを生成
            Vector3 bun = this.transform.position;
            bun.z += 3;
            Instantiate(prefab,bun, Quaternion.identity);
            audio.Play();
            count--;
        }
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
        if (hit.gameObject.tag == "Item")
        {
            if(count < 3)count++;
            Destroy(hit.gameObject);
        }
    }
}
