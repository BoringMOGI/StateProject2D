using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : CharactorController
{
    [SerializeField] Transform eyePivot;
    [SerializeField] float eyeRange;
    [SerializeField] LayerMask eyeMask;

    [Header("Input")]
    [SerializeField] bool isInputLeft;      // 왼쪽 입력을 했는가?

    AttackableEnemy attackable;

    private new void Start()
    {
        base.Start();
        attackable = GetComponent<AttackableEnemy>();
    }

    private void Update()
    {
        CheckWall();
        CheckFall();

        Attack();

        if(!isAttack)
            Movement(isInputLeft ? -1 : 1);
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

    private void Attack()
    {
        // 내가 공격중이 아니면서 적을 탐지했을 경우.
        if (!isAttack && attackable.isSearchEnemy)
        {
            isAttack = true;                    // 공격 중인지?
            anim.SetTrigger("onAttack");        // 애니메이션 트리거.
            Movement(0);                        // 움직임 멈추기.
            //attackable.Attack(isInputLeft);     // 실제 공격하기.
        }
    }
    private void Movement(float inputX)
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
