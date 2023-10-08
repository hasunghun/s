using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Vector2 inputVec;
    public float speed;
    public Scanner scanner;
    public Hand[] hands;

    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator animator;
    public RuntimeAnimatorController[] animCon;


    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        scanner = GetComponent<Scanner>();
        hands = GetComponentsInChildren<Hand>(true); // ��Ȱ��ȭ�� ������Ʈ�� �����Ͽ� �����´�.
    }
    private void OnEnable()
    {
        speed *= Character.Speed;
        animator.runtimeAnimatorController = animCon[GameManager.Instance.playerId];
    }
    private void FixedUpdate()
    {
        if (!GameManager.Instance.isLive)
            return;
        Vector2 nextVec = inputVec * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
    }
    private void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>();
    }
    //�������� ���� �Ǳ� �� ����Ǵ� �Լ�
    private void LateUpdate()
    {
        if (!GameManager.Instance.isLive)
            return;
        animator.SetFloat("Speed", inputVec.magnitude);
        //������ ũ��

        if (inputVec.x != 0)
        {
            spriteRenderer.flipX = inputVec.x < 0;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!GameManager.Instance.isLive)
            return;

        GameManager.Instance.health -= Time.deltaTime * 10;

        if (GameManager.Instance.health < 0)
        {
            //shadow�� area�� ����ΰ� ������ ��Ȱ��ȭ
            for (int index = 2; index < transform.childCount; index++)
            {
                transform.GetChild(index).gameObject.SetActive(false);
            }
            animator.SetTrigger("Dead");
            GameManager.Instance.GameOver();
        }
    }
}



/*
public class Player : MonoBehaviour
{
    //public���� ���� �� Add Component ��� �� Input Vec�� ����
    public Vector2 inputVec;
    public float speed;

    Rigidbody2D rigid;


    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }



    void Update()
    {
        //Input Ŭ���� = ����Ƽ���� �޴� ��� �Է��� �����ϴ� Ŭ����
        //Project Settint-Input Manager���� Ȯ��
        inputVec.x = Input.GetAxisRaw("Horizontal");
        inputVec.y = Input.GetAxisRaw("Vertical");
        //�׳� GetAxis���� ������ ��. 
    }

    //������ ���ؼ��� FixedUpdate
    private void FixedUpdate()
    {
        Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime;
        //normalized = �밢������ �̵��� �� �ӵ��� �����ϴ� ���� ����
        //fixed delta time = ���� ������ �ϳ��� �Һ��� �ð�
        //delta time = update���� ���

        //�̵����
        //1.���� �ش�
        //rigid.AddForce(inputVec);

        //2.�ӵ� ����
        //rigid.velocity = inputVec;

        //3.��ġ �̵�
        //�Ű������� ������� ��ġ�� �ޱ⶧���� �����־���Ѵ�.
        rigid.MovePosition(rigid.position + nextVec);

        //������ 1. �ʹ� ����, 2.�����ӿ� ���� �̵��ӵ� �ٸ� �� ����
    }
}
*/