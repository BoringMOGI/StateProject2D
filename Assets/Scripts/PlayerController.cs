using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : CharactorController
{
    [SerializeField] UserInfoUI userInfo;       // ���� ���� UI.

    Attackable attackable;      // ���� ���� Ŭ����.
    Rigidbody rigid;            // ���� ó����.
    Collider2D collier2D;       // �浹ü.
    
    bool isLockMovement;        // �������� ������ �� ���°�?

    private new void Start()
    {
        base.Start();   // ���� Ŭ������ Startȣ��.

        // �� ������Ʈ�� �˻��Ѵ�.
        attackable = GetComponent<Attackable>();
        rigid = GetComponent<Rigidbody>();
        collier2D = GetComponent<Collider2D>();

        OnUpdateUserInfo();
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

    // �̺�Ʈ �Լ�.
    public void OnDamaged(Transform attacker, int power)
    {
        if (isAlive == false)
            return;

        stat.hp = Mathf.Clamp(stat.hp - power, 0, stat.maxHp);      // ü�� ����.
        OnUpdateUserInfo();                                         // UI ������Ʈ.
        OnEndAttack();                                              // ���� ����.

        if (stat.hp <= 0)
        {
            anim.SetTrigger("onDead");
            Time.timeScale = 0.5f;
            Invoke(nameof(ResetTimeScale), 2f * Time.timeScale);    // Invoke�� n�� �ڿ� ���ϴ� �Լ��� ȣ���Ѵ�.
        }
        else
        {
            anim.SetTrigger("onDamage");

            // ���� ���� ������ �����ؼ� ����������.
            bool isLeftTarget = transform.position.x > attacker.position.x;
            Vector2 dir = new Vector2(isLeftTarget ? 1 : -1, 1);
            movement.Throw(dir, 1.5f);
            isLockMovement = true;

            // ���ư� ���� ���� �����ߴ��� üũ�ϴ� �ڷ�ƾ.
            StartCoroutine(CheckEndThrow());
        }
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

    public void OnUpdateUserInfo()
    {
        userInfo.Setup(stat.name);                  // ���� ���� UI�� �̸� ����.
        userInfo.UpdateHp(stat.hp, stat.maxHp);     // ���� ���� UI�� ���� ü�°� �ִ� ü�� ����.
    }


}
