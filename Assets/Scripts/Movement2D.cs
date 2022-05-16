using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement2D : MonoBehaviour
{
    public Animator anim;                   // Ư�� �ִϸ����͸� ������ ����.
    public SpriteRenderer spriteRenderer;   // Ư�� ��������Ʈ �������� ������ ����.
    public Rigidbody2D rigid;               // ���� ó�� ������Ʈ.

    public Transform groundPivot;   // ���� üũ �߽���.
    public LayerMask groundMask;    // ���� ����ũ.
    public float groundRadius;      // ���� üũ ���� ������.

    public float moveSpeed;     // �����̴� �ӵ�.
    public float jumpPower;     // �����ϴ� ��.

    public bool isGrounded;     // ���� ���ִ°�?
    int jumpCount;              // ������ �� �ִ� Ƚ��.

    int maxJumpCount = 1;       // �ִ�� ������ �� �ִ� Ƚ��.
    
    bool isHaveJumpItem;        // ���� �߰� Ƚ�� ���� �������� �Ծ��°�?
    bool isLockMovement;        // �������� ������ �� ���°�?

    // �� �����Ӹ��� ȣ��Ǵ� �̺�Ʈ �Լ�.
    void Update()
    {
        CheckGround();

        // �̵� ��� ����� �ʾ��� ���.
        if (!isLockMovement)
        {
            Movement();
            Jump();
        }

        if(Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("���� ������ ȹ��!");
            isHaveJumpItem = true;
        }

        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("velocityY", rigid.velocity.y);
    }

    // �ܺ� �Լ�.
    public void OnLockMovment(bool isLock)
    {
        isLockMovement = isLock;
        rigid.velocity = new Vector2(0f, rigid.velocity.y);
    }


    // ���� �Լ�.
    private void CheckGround()
    {
        // ���� ��� ���϶��� �ٴ� üũ�� ���� �ʴ´�.
        if (rigid.velocity.y > 0f)
            return;

        // �Ʒ� �������� distance��ŭ ������ �߻��� �浹�� ��ü�� collider�� ������ ��� ���� ���ִٰ� �Ǵ�.
        //RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundDistance);

        // �� ����� �浹 ������ ����� �ش� ������ �浹�� ��ü�� collider�� ��ȯ�Ѵ�.
        // groundMask�� ������ �ش� Layer�� ���� �浹ü�� �浹 üũ�Ѵ�.
        Collider2D hitCollider = Physics2D.OverlapCircle(groundPivot.position, groundRadius, groundMask);
        isGrounded = hitCollider != null;
        if(isGrounded)
        {
            jumpCount = maxJumpCount;
            if (isHaveJumpItem)
                jumpCount += 1;
        }    
    }
    private void Movement()
    {
        // Input (����Ƽ���� �Է� ���� Ŭ����)
        // GetAxisRaw : -1 or 0 or 1�� ���� �����ϴ� �Լ�.
        // Horizontal(����) : ����(-1), ���Է�(0), ������(1)
        // Vertical(����) : �Ʒ���(-1), ���Է�(0), ����(1)
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(x, y, 0);

        // transform (Transform ������Ʈ�� �����ϰ� �ִ�.)
        // Translate(Vector3) : �ش� �������� �������� ��ȯ.
        // deltaTime : ���� �����Ӻ��� ���� �����ӱ��� �ɸ� �ð�.
        //transform.Translate(direction * 5f * Time.deltaTime);

        // velocity : ���� �ӵ�.
        // x�ุ Ű ����� �ӵ��� ���Ѵ�.
        rigid.velocity = new Vector2(x * moveSpeed, rigid.velocity.y);


        bool isMove = (direction != Vector3.zero);      // �����̰� �ִ���?
        anim.SetBool("isMove", isMove);                 // �ִϸ����� ������ �Ķ���� isMove�� ���� ����.

        // ���� ��ȯ.
        if (x <= -1)
            spriteRenderer.flipX = true;
        else if (x >= 1)
            spriteRenderer.flipX = false;
    }
    private void Jump(bool isForce = false)
    {
        // AddForce(Ư�� �������� ���� ���Ѵ�).
        // �������� 10��ŭ�� ���� ���Ѵ�.
        // ForceMode2D.Force : �δ�.
        // ForceMode2D.Impulse : ����.
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount > 0)
        {
            jumpCount--;
            isGrounded = false;
            rigid.velocity = new Vector2(rigid.velocity.x, 0f);                 // ���� �� y�� �ӵ��� 0���� �ʱ�ȭ.
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);        // �� �������� jumpPower��ŭ ���� ���Ѵ�.

            anim.SetTrigger("onJump");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        if(groundPivot != null && rigid.velocity.y <= 0f)
            Gizmos.DrawWireSphere(groundPivot.position, groundRadius);

        //Gizmos.DrawRay(transform.position, Vector2.down * groundDistance);
    }
}
