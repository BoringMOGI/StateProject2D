using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : CharactorController
{
    [SerializeField] UserInfoUI userInfo;   // ���� ���� UI.
    [SerializeField] int gem;              // �÷��̾� ���� ���.

    [Header("Event")]
    [SerializeField] UnityEvent OnAttackEvent;

    Attackable attackable;      // ���� ���� Ŭ����.
    bool isLockMovement;        // �������� ������ �� ���°�?

    private new void Start()
    {
        base.Start();   // ���� Ŭ������ Startȣ��.

        attackable = GetComponent<Attackable>();
        isLockMovement = false;

        OnUpdateUI();
    }
    private void Update()
    {
        // Input (����Ƽ���� �Է� ���� Ŭ����)
        // GetAxisRaw : -1 or 0 or 1�� ���� �����ϴ� �Լ�.
        // Horizontal(����) : ����(-1), ���Է�(0), ������(1)
        // Vertical(����) : �Ʒ���(-1), ���Է�(0), ����(1)
        inputX = Input.GetAxisRaw("Horizontal");

        // �������� �ƴϰ� ������� ��� �����.
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
        // ���� Ŭ������ ���� �Լ��� �������̵�.
        OnAttackEvent?.Invoke();
        attackable.Attack(movement.moveDirection == VECTOR.Left);
    }
    protected override void Movement(float x)
    {
        movement.Move(x);                           // ���� �������� ����ϴ� Movement2D�� x�Է� �� ����.
    }

    private void Jump(bool isForce = false)
    {   
        // ����Ű�� ������ ��, movement���� Jump�Լ��� ȣ��.
        // �̶� true�� ��ȯ�Ǿ� ���� �ִϸ��̼� Ʈ���� ���.
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



    // �̺�Ʈ �Լ�.
    protected override void Damaged(Transform attacker, int power)
    {
        // ���� ���� ������ �����ؼ� ����������.
        bool isLeftTarget = transform.position.x > attacker.position.x;
        Vector2 dir = new Vector2(isLeftTarget ? 1 : -1, 1);
        movement.Throw(dir, 1.5f);
        isLockMovement = true;

        // ���ư� ���� ���� �����ߴ��� üũ�ϴ� �ڷ�ƾ.
        StartCoroutine(CheckEndThrow());
    }
    protected override void Dead()
    {
        Time.timeScale = 0.5f;
        Invoke(nameof(ResetTimeScale), 2f * Time.timeScale);    // Invoke�� n�� �ڿ� ���ϴ� �Լ��� ȣ���Ѵ�.
    }

    private IEnumerator CheckEndThrow()
    {
        // ���� ���� ���� ���� ��� ��� �ݺ�.
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
        userInfo.Setup(stat.name);                  // ���� ���� UI�� �̸� ����.
        userInfo.UpdateHp(stat.hp, stat.maxHp);     // ���� ���� UI�� ���� ü�°� �ִ� ü�� ����.
        userInfo.UpdateGem(gem);                    // ������ �������� ����.
    }
}
