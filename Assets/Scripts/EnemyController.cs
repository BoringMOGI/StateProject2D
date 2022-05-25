using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyController : CharactorController
{
    [SerializeField] Transform hpPivot;

    [Header("Eye")]
    [SerializeField] Transform eyePivot;
    [SerializeField] float eyeRange;
    [SerializeField] LayerMask eyeMask;

    [SerializeField] UnityEvent OnAttackEvent;

    [Header("ItemPrefab")]
    [SerializeField] DropItem itemPrefab;

    Coroutine knockbackCoroutine;           // �˹� �ڷ�ƾ.
    AttackableEnemy attackable;             // ���� Ŭ����.

    bool isKnockback;                       // �˹� ���´�.
    bool isInputLeft;                       // ���� �Է��� �ߴ°�?

    private new void Start()
    {
        base.Start();
        attackable = GetComponent<AttackableEnemy>();
        wallRayOffset = GetComponent<CapsuleCollider2D>().size.x;

        HpBarUI hpBar = HpBarManager.Instance.GetHpBar();       // HP�Ŵ������Լ� hpBar�� �ϳ� �����´�.
        hpBar.Setup(hpPivot, stat);                             // �ش� hpBar�� �� ������ �����Ѵ�.
    }

    private void Update()
    {
        // ���� ���������� ������ ������Ʈ�� �����.
        if (isAlive == false || isKnockback)
            return;
        
        CheckWall();
        CheckFall();
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


    Vector2 wallRayPos;     // �� üũ ������ ���� ��ġ.
    Vector2 wallRayDir;     // �� üũ ������ ����.
    float wallRayOffset;    // �� üũ ������ ��ġ ���� ��.

    // �� üũ.
    private void CheckWall()
    {
        wallRayDir = isInputLeft ? Vector2.left : Vector2.right;                           // ���� �Է��� ������ ����.
        wallRayPos = eyePivot.position;
        wallRayPos += (wallRayDir * wallRayOffset);

        RaycastHit2D hit = Physics2D.Raycast(wallRayPos, wallRayDir, eyeRange, eyeMask);    // �ش� ���� �������� Ray�߻�.
        if (hit.collider != null)
        {
            isInputLeft = !isInputLeft;     // �Է� �� �ݴ�� ������.
            Debug.Log("���� �ִ�! �� ���� : " + hit.collider.name);
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

    protected override void Attack()
    {
        if(attackable.isSearchEnemy)
            base.Attack();
    }
    protected override void OnAttack()
    {
        attackable.Attack(movement.moveDirection == VECTOR.Left);
        OnAttackEvent?.Invoke();
    }
    protected override void Movement(float inputX)
    {
        this.inputX = inputX;
        movement.Move(inputX);
    }
    protected override void Damaged(Transform attacker, int power)
    {
        // ���� �ڷ�ƾ�� ���ư��� �ִٸ� ���.
        if (knockbackCoroutine != null)
            StopCoroutine(knockbackCoroutine);

        // ���ο� �ڷ�ƾ ���� �� ����.
        knockbackCoroutine = StartCoroutine(Knockback());
    }
    protected override void Dead()
    {
        
    }
    protected override void OnUpdateUI()
    {
        // hpUi.UpdateHp(stat.hp, stat.maxHp);
    }

    IEnumerator Knockback()
    {
        isKnockback = true;
        yield return new WaitForSeconds(1f);
        isKnockback = false;
    }
    private void OnDeadDestroy()
    {
        // ���Ͱ� �װ� ����� �� ������ ���� �� ���� ������.
        DropItem item = Instantiate(itemPrefab, transform.position, transform.rotation);
        item.ShowItem();

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        if (eyePivot != null)
        {
            Gizmos.color = Color.blue;

            // �� üũ Ray.
            Gizmos.DrawRay(wallRayPos, wallRayDir * eyeRange);

            // ���� üũ Ray.
            Vector2 dir = isInputLeft ? Vector2.left : Vector2.right;
            Vector2 position = eyePivot.position;
            position += (dir * eyeRange);
            Gizmos.DrawRay(position, Vector2.down * eyeRange * 2f);
        }
    }
}
