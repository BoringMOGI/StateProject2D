using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] Movement2D movement;

    [Header("Attack")]
    [SerializeField] Transform attackPivot;     // ���� üũ ����.
    [SerializeField] float attackRange;         // ���� ����.
    [SerializeField] int attackPower;           // ���ݷ�.

    bool isAttack;      // �������ΰ�?

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z) && !isAttack && movement.isGrounded)
        {
            OnStartAttack();
        }
    }

    public void OnStartAttack()
    {
        anim.SetTrigger("onAttack");
        movement.OnLockMovment(true);
        isAttack = true;
    }
    public void OnEndAttack()
    {
        Debug.Log("OnEndAttack");
        movement.OnLockMovment(false);
        isAttack = false;
    }
    private void OnAttack()
    {
        // attackPivot�� �߽����� attackRagne �������� ���� ������ �浹 üũ �� ���� ��ȯ.
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPivot.position, attackRange);
        foreach(Collider2D hit in hits)
        {
            // �浹 üũ�� ��󿡰Լ� Damageable ������Ʈ�� �˻�.
            Damageable target = hit.GetComponent<Damageable>();
            if (target != null)
                target.OnDamaged(transform, attackPower);
        }
    }

    private void OnDrawGizmos()
    {
        if (attackPivot != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(attackPivot.position, attackRange);
        }
    }
}
