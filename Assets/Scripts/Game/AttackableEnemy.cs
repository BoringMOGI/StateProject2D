using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackableEnemy : Attackable
{
    [Header("Search")]
    [SerializeField] float searchRange;
    [SerializeField] float searchRadius;
    [SerializeField] LayerMask searchMask;
    [SerializeField] Movement2D movement;

    // ���� Ž���ߴ��� �˷��ִ� bool ����.
    public bool isSearchEnemy;

    private Vector2 GetSearchPosition()
    {
        Vector2 searchPosition = attackPivot.position;
        bool isLeft = movement.moveDirection == VECTOR.Left;
        searchPosition += (isLeft ? Vector2.left : Vector2.right) * searchRange;
        return searchPosition;
    }

    private void Update()
    {
        // Range��ŭ�� ���� overlap�ؼ� ���� Ž��.
        Collider2D collider = Physics2D.OverlapCircle(GetSearchPosition(), searchRadius, searchMask);
        isSearchEnemy = (collider != null);
    }

    private new void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();

        if (attackPivot)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(GetSearchPosition(), searchRadius);
        }
    }
}
