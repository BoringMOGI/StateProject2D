using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Status))]
public class Attackable : MonoBehaviour
{    
    [SerializeField] protected Transform attackPivot;   // ���� üũ ����.
    [SerializeField] private LayerMask attackMask;      // ���� ��� ����ũ.
    [SerializeField] private float attackRange;         // ���� �Ÿ�
    [SerializeField] private float attackRadius;        // ���� ����
    [SerializeField] private bool isLeft;               // ���� �����ΰ�?

    private Status stat;

    private void Start()
    {
        stat = GetComponent<Status>();
    }

    private Vector2 GetAttackPosition()
    {
        // attackPivot��ġ���� ���� Ȥ�� ���������� range �Ÿ� ��ŭ�� ��ġ�� ����� ��ȯ.
        Vector2 attackPosition = attackPivot.position;
        attackPosition += (isLeft ? Vector2.left : Vector2.right) * attackRange;

        return attackPosition;
    }

    public void Attack(bool isLeft)
    {
        this.isLeft = isLeft;
        Collider2D[] hits = Physics2D.OverlapCircleAll(GetAttackPosition(), attackRadius, attackMask);
        foreach (Collider2D hit in hits)
        {
            // �浹 üũ�� ��󿡰Լ� Damageable ������Ʈ�� �˻�.
            Damageable target = hit.GetComponent<Damageable>();
            if (target != null)
                target.OnDamaged(transform, stat.power);
        }
    }


    // OnDrawGizmos : ��� ����� �׸���
    // OnDrawGizmosSelected : �ش� ������Ʈ�� �����ؾ� ����� �׸���.
    protected void OnDrawGizmosSelected()
    {
        if (attackPivot != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(GetAttackPosition(), attackRadius);
        }
    }
}
