using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;//�÷��̾��� ������ ���ǵ�, ����, ���� ����
    float hAxis;
    float vAxis;
    bool wDown;
    bool jDown;

    bool isJump;

    Vector3 moveVec;//�÷��̾��� ������ ���Ͱ� ���� ����

    Animator anim;//�÷��̾��� ������ �ִϸ��̼� ���� ����
    Rigidbody rigid;

    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rigid = GetComponent<Rigidbody>();
    }

    void Update()
    {
        //transform.LookAt(transform.position + moveVec);//�÷��̾��� ȸ���� ����(�����ϰ� ������)
        GetInput();
        Move();
        Turn();
        Jump();
    }
    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        wDown = Input.GetButton("Walk");//����ƮŰ�� �� ������ �����̸� �޸���
        jDown = Input.GetButtonDown("Jump");//�����̽��ٸ� ������ ����
    }

    void Move()
    {
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;//�÷��̾��� ������ ���Ͱ� ����

        //�÷��̾��� �޸��� �ӵ� 0.3���� ����(���׿����� ����� ���)
        transform.position += moveVec * speed * (wDown ? 0.3f : 1f) * Time.deltaTime;

        //�÷��̾��� �޸��� �ӵ� 0.3���� ����(if���� ����� ���)
        //if(wDown)
        //    transform.position += moveVec * speed * 0.3f * Time.deltaTime;
        //else
        //    transform.position += moveVec * speed * Time.deltaTime;

        anim.SetBool("isRun", moveVec != Vector3.zero);//�÷��̾��� �޸��� ���Ͱ� ����
        anim.SetBool("isWalk", wDown);//�÷��̾��� �޸��� ���Ͱ� ����
    }

    void Turn()
    {
        if (moveVec != Vector3.zero)//�÷��̾��� ȸ���� ����(�ε巴�� ������)
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
            rigid.AddForce(Vector3.up * 15, ForceMode.Impulse);//���� �������� ����
            anim.SetBool("isJump", true);
            anim.SetTrigger("doJump");
            isJump = true;
        }
    }

    private void OnCollisionEnter(Collision collision)//ĳ���� �浹 ����
    {
        if(collision.gameObject.tag == "Floor")
        {
            anim.SetBool("isJump", false);
            isJump = false;
        }
    }
}
