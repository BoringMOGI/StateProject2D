using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum VECTOR
{
    Left,
    Right,
}

public class Movement2D : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;   // Ư�� ��������Ʈ �������� ������ ����.
    [SerializeField] Rigidbody2D rigid;               // ���� ó�� ������Ʈ.
    [SerializeField] Transform groundPivot;   // ���� üũ �߽���.
    [SerializeField] LayerMask groundMask;    // ���� ����ũ.

    [SerializeField] float groundRadius;      // ���� üũ ���� ������.
    [SerializeField] float moveSpeed;     // �����̴� �ӵ�.
    [SerializeField] float jumpPower;     // �����ϴ� ��.

    public bool isGrounded;       // ���� ���ִ°�?
    public VECTOR moveDirection;  // �ٶ󺸴� ����.

    public float velocityY => rigid.velocity.y;     // y�� �̵� �ӵ�.

    int jumpCount;              // ������ �� �ִ� Ƚ��.
    int maxJumpCount = 1;       // �ִ�� ������ �� �ִ� Ƚ��.
    bool isOriginLeft;          // ���ʿ� ������ ���� �ִ°�?

    private void Start()
    {
        // ���ʿ� ������ ���� �ִ��Ŀ� ���� spriteRenderer�� filpX�� �޶����� ������
        isOriginLeft = (moveDirection == VECTOR.Left);      
    }

    // �� �����Ӹ��� ȣ��Ǵ� �̺�Ʈ �Լ�.
    void Update()
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
        if (isGrounded)
            jumpCount = maxJumpCount;
    }

    // �ܺ� �Լ�.
    public void Move(float x)
    {
        // velocity : ���� �ӵ�.
        // x�ุ Ű ����� �ӵ��� ���Ѵ�.
        rigid.velocity = new Vector2(x * moveSpeed, rigid.velocity.y);     

        // ���� ��ȯ.
        if(x != 0)
        {
            if (isOriginLeft)
                spriteRenderer.flipX = (x >= 1);
            else
                spriteRenderer.flipX = (x <= -1);

            moveDirection = (x <= -1) ? VECTOR.Left : VECTOR.Right;
        }    
    }
    public bool Jump()
    {
        if (jumpCount <= 0)
            return false;

        // AddForce(Ư�� �������� ���� ���Ѵ�).
        // �������� 10��ŭ�� ���� ���Ѵ�.
        // ForceMode2D.Force : �δ�.
        // ForceMode2D.Impulse : ����.
        jumpCount--;
        isGrounded = false;
        rigid.velocity = new Vector2(rigid.velocity.x, 0f);                 // ���� �� y�� �ӵ��� 0���� �ʱ�ȭ.
        rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);        // �� �������� jumpPower��ŭ ���� ���Ѵ�.
        return true;
    }
    public void OnStopForce()
    {
        rigid.velocity = new Vector2(0f, rigid.velocity.y);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        if(groundPivot != null && rigid.velocity.y <= 0f)
            Gizmos.DrawWireSphere(groundPivot.position, groundRadius);

        //Gizmos.DrawRay(transform.position, Vector2.down * groundDistance);
    }
}
