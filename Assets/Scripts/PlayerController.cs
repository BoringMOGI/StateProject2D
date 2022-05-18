using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : CharactorController
{
    [SerializeField] UserInfoUI userInfo;       // 유저 정보 UI.
   
    bool isLockMovement;       // 움직임을 제어할 수 없는가?

    private new void Start()
    {
        base.Start();   // 상위 클래스의 Start호출.
        userInfo.Setup(stat.name);                  // 유저 정보 UI에 이름 전달.
        userInfo.UpdateHp(stat.hp, stat.maxHp);     // 유저 정보 UI에 현재 체력과 최대 체력 전달.
    }
    private void Update()
    {
        // Input (유니티에서 입력 관련 클래스)
        // GetAxisRaw : -1 or 0 or 1의 값을 리턴하는 함수.
        // Horizontal(수평) : 왼쪽(-1), 비입력(0), 오른쪽(1)
        // Vertical(수직) : 아래쪽(-1), 비입력(0), 위쪽(1)
        inputX = Input.GetAxisRaw("Horizontal");

        if (isAttack == false)
        {
            Movement(inputX);
            Jump();
            Attack();
        }
    }

    public void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Z) && !isAttack && movement.isGrounded)
        {
            anim.SetTrigger("onAttack");
            isAttack = true;
            Movement(0);
        }
    }
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
