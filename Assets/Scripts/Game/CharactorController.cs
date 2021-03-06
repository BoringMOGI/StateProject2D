using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharactorController : MonoBehaviour
{
    protected Animator anim;             // 애니메이터.
    protected Status stat;               // 캐릭의 상태 정보.
    protected Movement2D movement;       // 이동 관련 클래스.
    protected Collider2D collider2D;     // 충돌체.
    protected Rigidbody2D rigid;         // 물리 연산 클래스.

    protected bool isAttack;             // 공격중인가?
    protected float inputX;              // x축 방향 입력 값.

    protected bool isAlive => stat.hp > 0;

    protected void Start()
    {
        // 나의 오브젝트에서 해당 컴포넌트를 검색해 대입.
        anim = GetComponent<Animator>();
        stat = GetComponent<Status>();
        movement = GetComponent<Movement2D>();
        collider2D = GetComponent<Collider2D>();
        rigid = GetComponent<Rigidbody2D>();

        OnUpdateUI();
    }
    protected void LateUpdate()
    {
        // 애니메이터의 파라미터 갱신.
        anim.SetBool("isMove", isAttack == false && inputX != 0);   // 공격 중이 아니고 x입력 값이 0이 아닐 때 움직이고 있다고 판단.
        anim.SetBool("isGrounded", movement.isGrounded);            // 이동 관련 클래스의 지면 체크 값 전달.
        anim.SetFloat("velocityY", movement.velocityY);             // 이동 관련 클래스의 y축 속도 전달.
    }

    // 애니메이션 이벤트 함수.
    protected virtual void Attack()
    {
        // 내가 공격중이 아니면.
        if (!isAttack)
        {
            isAttack = true;                    // 공격 중인지?
            anim.SetTrigger("onAttack");        // 애니메이션 트리거.
            Movement(0);                        // 움직임 멈추기.
        }
    }
    private void OnEndAttack()
    {
        isAttack = false;
    }
    public void OnDamaged(Transform attacker, int power)
    {
        if (isAlive == false)
            return;

        stat.hp = Mathf.Clamp(stat.hp - power, 0, stat.maxHp);
        OnEndAttack();
        OnUpdateUI();

        if(isAlive)
        {
            anim.SetTrigger("onDamage");
            Damaged(attacker, power);
        }
        else
        {
            anim.SetTrigger("onDead");
            Dead();
        }
    }

    protected abstract void OnUpdateUI();
    protected abstract void Damaged(Transform attacker, int power);
    protected abstract void Dead();
    protected abstract void OnAttack();
    protected abstract void Movement(float inputX);
}
