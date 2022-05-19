using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : CharactorController
{
    [SerializeField] UserInfoUI userInfo;       // ���� ���� UI.

    Attackable attackable;     // ���� ���� Ŭ����.
    bool isLockMovement;       // �������� ������ �� ���°�?

    private new void Start()
    {
        base.Start();   // ���� Ŭ������ Startȣ��.

        attackable = GetComponent<Attackable>();    // �� ������Ʈ�� �˻��Ѵ�.
        OnUpdateUserInfo();
    }
    private void Update()
    {
        // Input (����Ƽ���� �Է� ���� Ŭ����)
        // GetAxisRaw : -1 or 0 or 1�� ���� �����ϴ� �Լ�.
        // Horizontal(����) : ����(-1), ���Է�(0), ������(1)
        // Vertical(����) : �Ʒ���(-1), ���Է�(0), ����(1)
        inputX = Input.GetAxisRaw("Horizontal");

        // �������� �ƴϰ� ������� ��� �����.
        if (!isAttack && isAlive)
        {
            Movement(inputX);
            Jump();
            Attack();
        }
    }

    public void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Z) && !isAttack && movement.isGrounded)
        {
            anim.SetTrigger("onAttack");
            isAttack = true;
            Movement(0);
        }
    }
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

    // �̺�Ʈ �Լ�.
    public void OnDead()
    {
        anim.SetTrigger("onDead");
    }
    public void OnUpdateUserInfo()
    {
        userInfo.Setup(stat.name);                  // ���� ���� UI�� �̸� ����.
        userInfo.UpdateHp(stat.hp, stat.maxHp);     // ���� ���� UI�� ���� ü�°� �ִ� ü�� ����.
    }

    protected override void OnAttack()
    {
        // ���� Ŭ������ ���� �Լ��� �������̵�.
        attackable.Attack(movement.moveDirection == VECTOR.Left);
    }
}
