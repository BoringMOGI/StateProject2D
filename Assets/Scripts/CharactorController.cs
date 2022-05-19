using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactorController : MonoBehaviour
{
    protected Animator anim;             // 애니메이터.
    protected Status stat;               // 캐릭의 상태 정보.
    protected Movement2D movement;       // 이동 관련 클래스.
    
    protected bool isAttack;             // 공격중인가?
    protected float inputX;              // x축 방향 입력 값.

    protected bool isAlive => stat.hp > 0;

    protected void Start()
    {
        // 나의 오브젝트에서 해당 컴포넌트를 검색해 대입.
        anim = GetComponent<Animator>();
        stat = GetComponent<Status>();
        movement = GetComponent<Movement2D>();
    }
    protected void LateUpdate()
    {
        // 애니메이터의 파라미터 갱신.
        anim.SetBool("isMove", isAttack == false && inputX != 0);   // 공격 중이 아니고 x입력 값이 0이 아닐 때 움직이고 있다고 판단.
        anim.SetBool("isGrounded", movement.isGrounded);            // 이동 관련 클래스의 지면 체크 값 전달.
        anim.SetFloat("velocityY", movement.velocityY);             // 이동 관련 클래스의 y축 속도 전달.
    }

    // 애니메이션 이벤트 함수.
    private void OnEndAttack()
    {
        isAttack = false;
    }
    protected virtual void OnAttack()
    {
        //attackable.Attack(movement.moveDirection == VECTOR.Left);
    }
}
