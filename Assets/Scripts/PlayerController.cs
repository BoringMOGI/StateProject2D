using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] UserInfoUI userInfo;       // ���� ���� UI.

    Animator anim;             // �ִϸ�����.
    Status stat;               // ĳ���� ���� ����.
    Movement2D movement;       // �̵� ���� Ŭ����.
    Attackable attackable;     // ���� ���� Ŭ����.

    bool isAttack;             // �������ΰ�?
    bool isLockMovement;       // �������� ������ �� ���°�?

    private void Start()
    {
        // ���� ������Ʈ���� �ش� ������Ʈ�� �˻��� ����.
        anim = GetComponent<Animator>();
        stat = GetComponent<Status>();
        movement = GetComponent<Movement2D>();
        attackable = GetComponent<Attackable>();

        userInfo.Setup(stat.name);                  // ���� ���� UI�� �̸� ����.
        userInfo.UpdateHp(stat.hp, stat.maxHp);     // ���� ���� UI�� ���� ü�°� �ִ� ü�� ����.
    }
    private void Update()
    {
        // Input (����Ƽ���� �Է� ���� Ŭ����)
        // GetAxisRaw : -1 or 0 or 1�� ���� �����ϴ� �Լ�.
        // Horizontal(����) : ����(-1), ���Է�(0), ������(1)
        // Vertical(����) : �Ʒ���(-1), ���Է�(0), ����(1)
        float x = Input.GetAxisRaw("Horizontal");

        if (isAttack == false)
        {
            Movement(x);
            Jump();
            Attack();
        }

        // �ִϸ������� �Ķ���� ����.
        anim.SetBool("isMove", isAttack == false && x != 0);        // ���� ���� �ƴϰ� x�Է� ���� 0�� �ƴ� �� �����̰� �ִٰ� �Ǵ�.
        anim.SetBool("isGrounded", movement.isGrounded);            // �̵� ���� Ŭ������ ���� üũ �� ����.
        anim.SetFloat("velocityY", movement.velocityY);             // �̵� ���� Ŭ������ y�� �ӵ� ����.
    }

    // ����.
    public void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Z) && !isAttack && movement.isGrounded)
        {
            anim.SetTrigger("onAttack");
            isAttack = true;
        }
    }
    public void OnEndAttack()
    {
        isAttack = false;
    }   
    private void OnAttack()
    {
        attackable.Attack(movement.moveDirection == VECTOR.Left);
    }

    // �̵�, ����.

    private void Movement(float x)
    {
        movement.Move(x);                           // ���� �������� ����ϴ� Movement2D�� x�Է� �� ����.
    }
    private void Jump(bool isForce = false)
    {   
        // ����Ű�� ������ ��, movement���� Jump�Լ��� ȣ��.
        // �̶� true�� ��ȯ�Ǿ� ���� �ִϸ��̼� Ʈ���� ���.
        if (Input.GetKeyDown(KeyCode.Space) && movement.Jump())
        {
            anim.SetTrigger("onJump");
        }
    }
}
