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

    Coroutine knockbackCoroutine;           // 넉백 코루틴.
    AttackableEnemy attackable;             // 공격 클래스.

    bool isKnockback;                       // 넉백 상태다.
    bool isInputLeft;                       // 왼쪽 입력을 했는가?

    private new void Start()
    {
        base.Start();
        attackable = GetComponent<AttackableEnemy>();
        wallRayOffset = GetComponent<CapsuleCollider2D>().size.x;

        HpBarUI hpBar = HpBarManager.Instance.GetHpBar();       // HP매니저에게서 hpBar를 하나 꺼내온다.
        hpBar.Setup(hpPivot, stat);                             // 해당 hpBar에 내 정보를 세팅한다.
    }

    private void Update()
    {
        // 내가 생존해있지 않으면 업데이트를 멈춘다.
        if (isAlive == false || isKnockback)
            return;
        
        CheckWall();
        CheckFall();
        Attack();

        if(!isAttack)
            Movement(isInputLeft ? -1 : 1);
    }

    // 애니메이션 파라미터 동기화.
    private new void LateUpdate()
    {
        base.LateUpdate();
        anim.SetBool("isKnockback", isKnockback);
    }


    Vector2 wallRayPos;     // 벽 체크 광선의 시작 위치.
    Vector2 wallRayDir;     // 벽 체크 광선의 방향.
    float wallRayOffset;    // 벽 체크 광선의 위치 수정 값.

    // 벽 체크.
    private void CheckWall()
    {
        wallRayDir = isInputLeft ? Vector2.left : Vector2.right;                           // 내가 입력한 방향의 벡터.
        wallRayPos = eyePivot.position;
        wallRayPos += (wallRayDir * wallRayOffset);

        RaycastHit2D hit = Physics2D.Raycast(wallRayPos, wallRayDir, eyeRange, eyeMask);    // 해당 벡터 방향으로 Ray발사.
        if (hit.collider != null)
        {
            isInputLeft = !isInputLeft;     // 입력 값 반대로 돌리기.
            Debug.Log("무언가 있다! 뒤 돌자 : " + hit.collider.name);
        }
    }
    private void CheckFall()
    {
        Vector2 dir = isInputLeft ? Vector2.left : Vector2.right;                           // 내가 입력한 방향의 벡터.
        Vector2 position = eyePivot.position;
        position += (dir * eyeRange);

        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.down, eyeRange * 2f, eyeMask);
        if(hit.collider == null)
        {
            isInputLeft = !isInputLeft;
            Debug.Log("절벽이 있다. 뒤로 돌자!");
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
        // 이전 코루틴이 돌아가고 있다면 취소.
        if (knockbackCoroutine != null)
            StopCoroutine(knockbackCoroutine);

        // 새로운 코루틴 실행 후 대입.
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
        // 몬스터가 죽고 사라질 때 아이템 생성 후 위로 던지기.
        DropItem item = Instantiate(itemPrefab, transform.position, transform.rotation);
        item.ShowItem();

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        if (eyePivot != null)
        {
            Gizmos.color = Color.blue;

            // 벽 체크 Ray.
            Gizmos.DrawRay(wallRayPos, wallRayDir * eyeRange);

            // 절벽 체크 Ray.
            Vector2 dir = isInputLeft ? Vector2.left : Vector2.right;
            Vector2 position = eyePivot.position;
            position += (dir * eyeRange);
            Gizmos.DrawRay(position, Vector2.down * eyeRange * 2f);
        }
    }
}
