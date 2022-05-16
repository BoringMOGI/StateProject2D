using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] Movement2D movement;

    [Header("Attack")]
    [SerializeField] Transform attackPivot;     // 공격 체크 지점.
    [SerializeField] float attackRange;         // 공격 범위.
    [SerializeField] int attackPower;           // 공격력.

    bool isAttack;      // 공격중인가?

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
        // attackPivot을 중심으로 attackRagne 반지름의 원을 생성해 충돌 체크 후 전부 반환.
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPivot.position, attackRange);
        foreach(Collider2D hit in hits)
        {
            // 충돌 체크한 대상에게서 Damageable 컴포넌트를 검색.
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
