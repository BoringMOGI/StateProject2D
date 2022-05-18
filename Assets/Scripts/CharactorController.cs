using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactorController : MonoBehaviour
{
    protected Animator anim;             // �ִϸ�����.
    protected Status stat;               // ĳ���� ���� ����.
    protected Movement2D movement;       // �̵� ���� Ŭ����.
    protected Attackable attackable;     // ���� ���� Ŭ����.

    protected bool isAttack;             // �������ΰ�?
    protected float inputX;              // x�� ���� �Է� ��.

    protected void Start()
    {
        // ���� ������Ʈ���� �ش� ������Ʈ�� �˻��� ����.
        anim = GetComponent<Animator>();
        stat = GetComponent<Status>();
        movement = GetComponent<Movement2D>();
        attackable = GetComponent<Attackable>();
    }
    protected void LateUpdate()
    {
        // �ִϸ������� �Ķ���� ����.
        anim.SetBool("isMove", isAttack == false && inputX != 0);   // ���� ���� �ƴϰ� x�Է� ���� 0�� �ƴ� �� �����̰� �ִٰ� �Ǵ�.
        anim.SetBool("isGrounded", movement.isGrounded);            // �̵� ���� Ŭ������ ���� üũ �� ����.
        anim.SetFloat("velocityY", movement.velocityY);             // �̵� ���� Ŭ������ y�� �ӵ� ����.
    }

    // �ִϸ��̼� �̺�Ʈ �Լ�.
    private void OnEndAttack()
    {
        isAttack = false;
    }
    private void OnAttack()
    {
        attackable.Attack(movement.moveDirection == VECTOR.Left);
    }
}
