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
    [SerializeField] SpriteRenderer spriteRenderer;   // 특정 스프라이트 렌더러를 참조할 변수.
    [SerializeField] Rigidbody2D rigid;               // 물리 처리 컴포넌트.
    [SerializeField] Transform groundPivot;   // 지면 체크 중심점.
    [SerializeField] LayerMask groundMask;    // 지면 마스크.

    [SerializeField] float groundRadius;      // 지면 체크 원의 반지름.
    [SerializeField] float moveSpeed;     // 움직이는 속도.
    [SerializeField] float jumpPower;     // 점프하는 힘.

    public bool isGrounded;       // 땅에 서있는가?
    public VECTOR moveDirection;  // 바라보는 방향.

    public float velocityY => rigid.velocity.y;     // y축 이동 속도.

    int jumpCount;              // 점프할 수 있는 횟수.
    int maxJumpCount = 1;       // 최대로 점프할 수 있는 횟수.
    bool isOriginLeft;          // 최초에 왼쪽을 보고 있는가?

    private void Start()
    {
        // 최초에 왼쪽을 보고 있느냐에 따라서 spriteRenderer의 filpX가 달라지기 때문에
        isOriginLeft = (moveDirection == VECTOR.Left);      
    }

    // 매 프레임마다 호출되는 이벤트 함수.
    void Update()
    {
        // 내가 상승 중일때는 바닥 체크를 하지 않는다.
        if (rigid.velocity.y > 0f)
            return;

        // 아래 방향으로 distance만큼 광선을 발사해 충돌한 물체의 collider가 존재할 경우 땅에 서있다고 판단.
        //RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundDistance);

        // 원 모양의 충돌 영역을 만들고 해당 영역에 충돌한 물체의 collider를 반환한다.
        // groundMask를 넣으면 해당 Layer를 가진 충돌체만 충돌 체크한다.
        Collider2D hitCollider = Physics2D.OverlapCircle(groundPivot.position, groundRadius, groundMask);
        isGrounded = hitCollider != null;
        if (isGrounded)
            jumpCount = maxJumpCount;
    }

    // 외부 함수.
    public void Move(float x)
    {
        // velocity : 현재 속도.
        // x축만 키 제어로 속도가 변한다.
        rigid.velocity = new Vector2(x * moveSpeed, rigid.velocity.y);     

        // 방향 전환.
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

        // AddForce(특정 방향으로 힘을 가한다).
        // 위쪽으로 10만큼의 힘을 가한다.
        // ForceMode2D.Force : 민다.
        // ForceMode2D.Impulse : 찬다.
        jumpCount--;
        isGrounded = false;
        rigid.velocity = new Vector2(rigid.velocity.x, 0f);                 // 현재 내 y축 속도를 0으로 초기화.
        rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);        // 위 방향으로 jumpPower만큼 힘을 가한다.
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
