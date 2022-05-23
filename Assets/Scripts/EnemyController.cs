using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : CharactorController
{
    [SerializeField] Transform hpPivot;

    [Header("Eye")]
    [SerializeField] Transform eyePivot;
    [SerializeField] float eyeRange;
    [SerializeField] LayerMask eyeMask;

    Coroutine knockbackCoroutine;           // 넉백 코루틴.
    AttackableEnemy attackable;             // 공격 클래스.
    bool isKnockback;                       // 넉백 상태다.
    bool isInputLeft;                       // 왼쪽 입력을 했는가?

    private new void Start()
    {
        base.Start();
        attackable = GetComponent<AttackableEnemy>();

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

    // 벽 체크.
    private void CheckWall()
    {        
        Vector2 dir = isInputLeft ? Vector2.left : Vector2.right;                           // 내가 입력한 방향의 벡터.
        RaycastHit2D hit = Physics2D.Raycast(eyePivot.position, dir, eyeRange, eyeMask);    // 해당 벡터 방향으로 Ray발사.
        if (hit.collider != null)
        {
            isInputLeft = !isInputLeft;     // 입력 값 반대로 돌리기.
            Debug.Log("벽이 있다! 뒤 돌자");
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
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        if (eyePivot != null)
        {
            Gizmos.color = Color.blue;

            // 벽 체크 Ray.
            Gizmos.DrawRay(eyePivot.position, (isInputLeft ? Vector2.left : Vector2.right) * eyeRange);

            // 절벽 체크 Ray.
            Vector2 dir = isInputLeft ? Vector2.left : Vector2.right;
            Vector2 position = eyePivot.position;
            position += (dir * eyeRange);
            Gizmos.DrawRay(position, Vector2.down * eyeRange * 2f);
        }
    }
}
