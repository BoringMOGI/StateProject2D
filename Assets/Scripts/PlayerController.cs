using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : CharactorController
{
    [SerializeField] UserInfoUI userInfo;       // ���� ���� UI.
   
    bool isLockMovement;       // �������� ������ �� ���°�?

    private new void Start()
    {
        base.Start();   // ���� Ŭ������ Startȣ��.
        userInfo.Setup(stat.name);                  // ���� ���� UI�� �̸� ����.
        userInfo.UpdateHp(stat.hp, stat.maxHp);     // ���� ���� UI�� ���� ü�°� �ִ� ü�� ����.
    }
    private void Update()
    {
        // Input (����Ƽ���� �Է� ���� Ŭ����)
        // GetAxisRaw : -1 or 0 or 1�� ���� �����ϴ� �Լ�.
        // Horizontal(����) : ����(-1), ���Է�(0), ������(1)
        // Vertical(����) : �Ʒ���(-1), ���Է�(0), ����(1)
        inputX = Input.GetAxisRaw("Horizontal");

        if (isAttack == false)
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
}
