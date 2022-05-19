using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Status))]
public class Attackable : MonoBehaviour
{    
    [SerializeField] protected Transform attackPivot;   // 공격 체크 지점.
    [SerializeField] private LayerMask attackMask;      // 공격 대상 마스크.
    [SerializeField] private float attackRange;         // 공격 거리
    [SerializeField] private float attackRadius;        // 공격 범위
    [SerializeField] private bool isLeft;               // 좌측 공격인가?

    private Status stat;

    private void Start()
    {
        stat = GetComponent<Status>();
    }

    private Vector2 GetAttackPosition()
    {
        // attackPivot위치에서 왼쪽 혹은 오른쪽으로 range 거리 만큼의 위치를 계산해 반환.
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
            // 충돌 체크한 대상에게서 Damageable 컴포넌트를 검색.
            Damageable target = hit.GetComponent<Damageable>();
            if (target != null)
                target.OnDamaged(transform, stat.power);
        }
    }


    // OnDrawGizmos : 상시 기즈모를 그린다
    // OnDrawGizmosSelected : 해당 오브젝트를 선택해야 기즈모를 그린다.
    protected void OnDrawGizmosSelected()
    {
        if (attackPivot != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(GetAttackPosition(), attackRadius);
        }
    }
}
