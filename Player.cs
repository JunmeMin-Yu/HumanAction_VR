using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;//플레이어의 움직임 스피드, 수직, 수평 선언
    float hAxis;
    float vAxis;
    bool wDown;
    bool jDown;

    bool isJump;

    Vector3 moveVec;//플레이어의 움직임 벡터값 변수 선언

    Animator anim;//플레이어의 움직임 애니메이션 변수 선언
    Rigidbody rigid;

    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rigid = GetComponent<Rigidbody>();
    }

    void Update()
    {
        //transform.LookAt(transform.position + moveVec);//플레이어의 회전값 설정(딱딱하게 움직임)
        GetInput();
        Move();
        Turn();
        Jump();
    }
    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        wDown = Input.GetButton("Walk");//시프트키를 꾹 누르고 움직이면 달리기
        jDown = Input.GetButtonDown("Jump");//스페이스바를 누르면 점프
    }

    void Move()
    {
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;//플레이어의 움직임 벡터값 설정

        //플레이어의 달리기 속도 0.3으로 설정(삼항연산자 사용할 경우)
        transform.position += moveVec * speed * (wDown ? 0.3f : 1f) * Time.deltaTime;

        //플레이어의 달리기 속도 0.3으로 설정(if문을 사용할 경우)
        //if(wDown)
        //    transform.position += moveVec * speed * 0.3f * Time.deltaTime;
        //else
        //    transform.position += moveVec * speed * Time.deltaTime;

        anim.SetBool("isRun", moveVec != Vector3.zero);//플레이어의 달리기 벡터값 설정
        anim.SetBool("isWalk", wDown);//플레이어의 달리기 벡터값 설정
    }

    void Turn()
    {
        if (moveVec != Vector3.zero)//플레이어의 회전값 설정(부드럽게 움직임)
        {
            Vector3 relativePos = (transform.position + moveVec) - transform.position;
            Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * 10);
        }
    }
    void Jump()
    {
        if (jDown && !isJump)
        {
            rigid.AddForce(Vector3.up * 15, ForceMode.Impulse);//위쪽 방향으로 점프
            anim.SetBool("isJump", true);
            anim.SetTrigger("doJump");
            isJump = true;
        }
    }

    private void OnCollisionEnter(Collision collision)//캐릭터 충돌 변수
    {
        if(collision.gameObject.tag == "Floor")
        {
            anim.SetBool("isJump", false);
            isJump = false;
        }
    }
}
