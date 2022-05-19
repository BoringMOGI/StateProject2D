using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : CharactorController
{
    [SerializeField] Transform eyePivot;
    [SerializeField] float eyeRange;
    [SerializeField] LayerMask eyeMask;
    [SerializeField] Collider2D collider;
    [SerializeField] Rigidbody2D rigid;

    [Header("Input")]
    [SerializeField] bool isInputLeft;      // ���� �Է��� �ߴ°�?

    AttackableEnemy attackable;

    private new void Start()
    {
        base.Start();
        attackable = GetComponent<AttackableEnemy>();
    }

    private void Update()
    {
        // ���� ���������� ������ ������Ʈ�� �����.
        if (isAlive == false)
            return;

        CheckWall();
        CheckFall();

        Attack();

        if(!isAttack)
            Movement(isInputLeft ? -1 : 1);
    }

    // �� üũ.
    private void CheckWall()
    {        
        Vector2 dir = isInputLeft ? Vector2.left : Vector2.right;                           // ���� �Է��� ������ ����.
        RaycastHit2D hit = Physics2D.Raycast(eyePivot.position, dir, eyeRange, eyeMask);    // �ش� ���� �������� Ray�߻�.
        if (hit.collider != null)
        {
            isInputLeft = !isInputLeft;     // �Է� �� �ݴ�� ������.
            Debug.Log("���� �ִ�! �� ����");
        }
    }
    private void CheckFall()
    {
        Vector2 dir = isInputLeft ? Vector2.left : Vector2.right;                           // ���� �Է��� ������ ����.
        Vector2 position = eyePivot.position;
        position += (dir * eyeRange);

        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.down, eyeRange * 2f, eyeMask);
        if(hit.collider == null)
        {
            isInputLeft = !isInputLeft;
            Debug.Log("������ �ִ�. �ڷ� ����!");
        }
    }

    private void Attack()
    {
        // ���� �������� �ƴϸ鼭 ���� Ž������ ���.
        if (!isAttack && attackable.isSearchEnemy)
        {
            isAttack = true;                    // ���� ������?
            anim.SetTrigger("onAttack");        // �ִϸ��̼� Ʈ����.
            Movement(0);                        // ������ ���߱�.
        }
    }
    private void Movement(float inputX)
    {
        this.inputX = inputX;
        movement.Move(inputX);
    }

    private void OnDead()
    {
        anim.SetTrigger("onDead");      // �ִϸ��̼� Ʈ���� ����.
        movement.OnStopForce();         // ������ ���߱�.
        collider.enabled = false;       // �浹ü ����.
        rigid.isKinematic = true;       // ���� ���� ���� �ʰڴ�.
    }
    private void OnDeadDestroy()
    {
        Destroy(gameObject);            // �ڽ��� ������Ʈ�� �����Ѵ�.
    }

    // �ִϸ��̼� Ŭ������ �θ��� �̺�Ʈ �Լ�.
    protected override void OnAttack()
    {
        attackable.Attack(movement.moveDirection == VECTOR.Left);
    }


    private void OnDrawGizmosSelected()
    {
        if (eyePivot != null)
        {
            Gizmos.color = Color.blue;

            // �� üũ Ray.
            Gizmos.DrawRay(eyePivot.position, (isInputLeft ? Vector2.left : Vector2.right) * eyeRange);

            // ���� üũ Ray.
            Vector2 dir = isInputLeft ? Vector2.left : Vector2.right;
            Vector2 position = eyePivot.position;
            position += (dir * eyeRange);
            Gizmos.DrawRay(position, Vector2.down * eyeRange * 2f);
        }
    }
}
