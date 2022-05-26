using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharactorController : MonoBehaviour
{
    protected Animator anim;             // �ִϸ�����.
    protected Status stat;               // ĳ���� ���� ����.
    protected Movement2D movement;       // �̵� ���� Ŭ����.
    protected Collider2D collider2D;     // �浹ü.
    protected Rigidbody2D rigid;         // ���� ���� Ŭ����.

    protected bool isAttack;             // �������ΰ�?
    protected float inputX;              // x�� ���� �Է� ��.

    protected bool isAlive => stat.hp > 0;

    protected void Start()
    {
        // ���� ������Ʈ���� �ش� ������Ʈ�� �˻��� ����.
        anim = GetComponent<Animator>();
        stat = GetComponent<Status>();
        movement = GetComponent<Movement2D>();
        collider2D = GetComponent<Collider2D>();
        rigid = GetComponent<Rigidbody2D>();

        OnUpdateUI();
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
        // ���� �������� �ƴϸ�.
        if (!isAttack)
        {
            isAttack = true;                    // ���� ������?
            anim.SetTrigger("onAttack");        // �ִϸ��̼� Ʈ����.
            Movement(0);                        // ������ ���߱�.
        }
    }
    private void OnEndAttack()
    {
        isAttack = false;
    }
    public void OnDamaged(Transform attacker, int power)
    {
        if (isAlive == false)
            return;

        stat.hp = Mathf.Clamp(stat.hp - power, 0, stat.maxHp);
        OnEndAttack();
        OnUpdateUI();

        if(isAlive)
        {
            anim.SetTrigger("onDamage");
            Damaged(attacker, power);
        }
        else
        {
            anim.SetTrigger("onDead");
            Dead();
        }
    }

    protected abstract void OnUpdateUI();
    protected abstract void Damaged(Transform attacker, int power);
    protected abstract void Dead();
    protected abstract void OnAttack();
    protected abstract void Movement(float inputX);
}
