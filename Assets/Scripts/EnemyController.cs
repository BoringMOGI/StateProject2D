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
    [SerializeField] HpBarUI hpUi;

    [Header("Input")]
    [SerializeField] bool isInputLeft;      // ���� �Է��� �ߴ°�?

    AttackableEnemy attackable;             // ���� Ŭ����.
    bool isKnockback;                       // �˹� ���´�.
    Coroutine knockbackCoroutine;           // �˹� �ڷ�ƾ.

    private new void Start()
    {
        base.Start();
        attackable = GetComponent<AttackableEnemy>();
    }

    private void Update()
    {
        // ���� ���������� ������ ������Ʈ�� �����.
        if (isAlive == false || isKnockback)
            return;
        
        CheckWall();
        CheckFall();

        if(attackable.isSearchEnemy)
            Attack();

        if(!isAttack)
            Movement(isInputLeft ? -1 : 1);
    }

    // �ִϸ��̼� �Ķ���� ����ȭ.
    private new void LateUpdate()
    {
        base.LateUpdate();
        anim.SetBool("isKnockback", isKnockback);
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

  

    public void OnDamaged(Transform attacker, int power)
    {
        if (stat.hp <= 0)
            return;

        stat.hp = Mathf.Clamp(stat.hp - power, 0, stat.maxHp);      // ü�� ����.
        hpUi.UpdateHp(stat.hp, stat.maxHp);                         // ü�¹� ������Ʈ.
        movement.OnStopForce();

        if (stat.hp <= 0)
        {
            anim.SetTrigger("onDead");
        }
        else
        {
            anim.SetTrigger("onDamage");

            // ���� �ڷ�ƾ�� ���ư��� �ִٸ� ���.
            if (knockbackCoroutine != null)
                StopCoroutine(knockbackCoroutine);

            // ���ο� �ڷ�ƾ ���� �� ����.
            knockbackCoroutine = StartCoroutine(Knockback());
        }
    }
    private void OnDeadDestroy()
    {
        Destroy(gameObject);
    }
    IEnumerator Knockback()
    {
        isKnockback = true;
        yield return new WaitForSeconds(1f);
        isKnockback = false;
    }


    protected override void Movement(float inputX)
    {
        this.inputX = inputX;
        movement.Move(inputX);
    }
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
