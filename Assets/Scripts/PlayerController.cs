using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] UserInfoUI userInfo;       // 유저 정보 UI.

    Animator anim;             // 애니메이터.
    Status stat;               // 캐릭의 상태 정보.
    Movement2D movement;       // 이동 관련 클래스.
    Attackable attackable;     // 공격 관련 클래스.

    bool isAttack;             // 공격중인가?
    bool isLockMovement;       // 움직임을 제어할 수 없는가?

    private void Start()
    {
        // 나의 오브젝트에서 해당 컴포넌트를 검색해 대입.
        anim = GetComponent<Animator>();
        stat = GetComponent<Status>();
        movement = GetComponent<Movement2D>();
        attackable = GetComponent<Attackable>();

        userInfo.Setup(stat.name);                  // 유저 정보 UI에 이름 전달.
        userInfo.UpdateHp(stat.hp, stat.maxHp);     // 유저 정보 UI에 현재 체력과 최대 체력 전달.
    }
    private void Update()
    {
        // Input (유니티에서 입력 관련 클래스)
        // GetAxisRaw : -1 or 0 or 1의 값을 리턴하는 함수.
        // Horizontal(수평) : 왼쪽(-1), 비입력(0), 오른쪽(1)
        // Vertical(수직) : 아래쪽(-1), 비입력(0), 위쪽(1)
        float x = Input.GetAxisRaw("Horizontal");

        if (isAttack == false)
        {
            Movement(x);
            Jump();
            Attack();
        }

        // 애니메이터의 파라미터 갱신.
        anim.SetBool("isMove", isAttack == false && x != 0);        // 공격 중이 아니고 x입력 값이 0이 아닐 때 움직이고 있다고 판단.
        anim.SetBool("isGrounded", movement.isGrounded);            // 이동 관련 클래스의 지면 체크 값 전달.
        anim.SetFloat("velocityY", movement.velocityY);             // 이동 관련 클래스의 y축 속도 전달.
    }

    // 공격.
    public void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Z) && !isAttack && movement.isGrounded)
        {
            anim.SetTrigger("onAttack");
            isAttack = true;
        }
    }
    public void OnEndAttack()
    {
        isAttack = false;
    }   
    private void OnAttack()
    {
        attackable.Attack(movement.moveDirection == VECTOR.Left);
    }

    // 이동, 점프.

    private void Movement(float x)
    {
        movement.Move(x);                           // 실제 움직임을 담당하는 Movement2D에 x입력 값 전달.
    }
    private void Jump(bool isForce = false)
    {   
        // 점프키를 눌렀을 때, movement에게 Jump함수를 호출.
        // 이때 true가 반환되어 오면 애니메이션 트리거 재생.
        if (Input.GetKeyDown(KeyCode.Space) && movement.Jump())
        {
            anim.SetTrigger("onJump");
        }
    }
}
