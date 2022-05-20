using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharactorController : MonoBehaviour
{
    protected Animator anim;             // �ִϸ�����.
    protected Status stat;               // ĳ���� ���� ����.
    protected Movement2D movement;       // �̵� ���� Ŭ����.
    protected Attackable attackable;     // ���� ���� Ŭ����.
    
    protected bool isAttack;             // �������ΰ�?
    protected float inputX;              // x�� ���� �Է� ��.

    protected bool isAlive => stat.hp > 0;

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
    protected virtual void Attack()
    {
        // ���� �������� �ƴϸ鼭 ���� Ž������ ���.
        if (!isAttack)
        {
            isAttack = true;                    // ���� ������?
            anim.SetTrigger("onAttack");        // �ִϸ��̼� Ʈ����.
            Movement(0);                        // ������ ���߱�.
        }
    }
    protected void OnEndAttack()
    {
        isAttack = false;
    }

    protected abstract void OnAttack();
    protected abstract void Movement(float inputX);
}
