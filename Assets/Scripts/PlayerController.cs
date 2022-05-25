using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : CharactorController
{
    [SerializeField] UserInfoUI userInfo;   // 유저 정보 UI.
    [SerializeField] int gem;              // 플레이어 소지 골드.

    [Header("Event")]
    [SerializeField] UnityEvent OnAttackEvent;

    Attackable attackable;      // 공격 관련 클래스.
    bool isLockMovement;        // 움직임을 제어할 수 없는가?

    private new void Start()
    {
        base.Start();   // 상위 클래스의 Start호출.

        attackable = GetComponent<Attackable>();
        isLockMovement = false;

        OnUpdateUI();
    }
    private void Update()
    {
        // Input (유니티에서 입력 관련 클래스)
        // GetAxisRaw : -1 or 0 or 1의 값을 리턴하는 함수.
        // Horizontal(수평) : 왼쪽(-1), 비입력(0), 오른쪽(1)
        // Vertical(수직) : 아래쪽(-1), 비입력(0), 위쪽(1)
        inputX = Input.GetAxisRaw("Horizontal");

        // 공격중이 아니고 살아있을 경우 제어가능.
        if (!isAttack && isAlive && !isLockMovement)
        {
            Movement(inputX);
            Jump();
            Attack();
        }
    }

    protected override void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Z) && movement.isGrounded)
            base.Attack();
    }
    protected override void OnAttack()
    {
        // 상위 클래스의 가상 함수를 오버라이딩.
        OnAttackEvent?.Invoke();
        attackable.Attack(movement.moveDirection == VECTOR.Left);
    }
    protected override void Movement(float x)
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



    public void GetGem(int amount)
    {
        gem += amount;
        OnUpdateUI();
    }



    // 이벤트 함수.
    protected override void Damaged(Transform attacker, int power)
    {
        // 상대와 나의 방향을 생각해서 날려보낸다.
        bool isLeftTarget = transform.position.x > attacker.position.x;
        Vector2 dir = new Vector2(isLeftTarget ? 1 : -1, 1);
        movement.Throw(dir, 1.5f);
        isLockMovement = true;

        // 날아간 이후 땅에 착지했는지 체크하는 코루틴.
        StartCoroutine(CheckEndThrow());
    }
    protected override void Dead()
    {
        Time.timeScale = 0.5f;
        Invoke(nameof(ResetTimeScale), 2f * Time.timeScale);    // Invoke는 n초 뒤에 원하는 함수를 호출한다.
    }

    private IEnumerator CheckEndThrow()
    {
        // 내가 땅에 있지 않을 경우 계속 반복.
        while(!movement.isGrounded)
            yield return null;

        isLockMovement = false;
    }
    private void ResetTimeScale()
    {
        Time.timeScale = 1f;
    }

    protected override void OnUpdateUI()
    {
        userInfo.Setup(stat.name);                  // 유저 정보 UI에 이름 전달.
        userInfo.UpdateHp(stat.hp, stat.maxHp);     // 유저 정보 UI에 현재 체력과 최대 체력 전달.
        userInfo.UpdateGem(gem);                    // 유저의 보석양을 전달.
    }
}
