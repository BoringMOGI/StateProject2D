using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement2D : MonoBehaviour
{
    public Animator anim;                   // 특정 애니메이터를 참조할 변수.
    public SpriteRenderer spriteRenderer;   // 특정 스프라이트 렌더러를 참조할 변수.
    public Rigidbody2D rigid;               // 물리 처리 컴포넌트.

    public Transform groundPivot;   // 지면 체크 중심점.
    public LayerMask groundMask;    // 지면 마스크.
    public float groundRadius;      // 지면 체크 원의 반지름.

    public float moveSpeed;     // 움직이는 속도.
    public float jumpPower;     // 점프하는 힘.

    public bool isGrounded;     // 땅에 서있는가?
    int jumpCount;              // 점프할 수 있는 횟수.

    int maxJumpCount = 1;       // 최대로 점프할 수 있는 횟수.
    
    bool isHaveJumpItem;        // 점프 추가 횟수 증가 아이템을 먹었는가?
    bool isLockMovement;        // 움직임을 제어할 수 없는가?

    // 매 프레임마다 호출되는 이벤트 함수.
    void Update()
    {
        CheckGround();

        // 이동 제어가 잠기지 않았을 경우.
        if (!isLockMovement)
        {
            Movement();
            Jump();
        }

        if(Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("점프 아이템 획득!");
            isHaveJumpItem = true;
        }

        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("velocityY", rigid.velocity.y);
    }

    // 외부 함수.
    public void OnLockMovment(bool isLock)
    {
        isLockMovement = isLock;
        rigid.velocity = new Vector2(0f, rigid.velocity.y);
    }


    // 내부 함수.
    private void CheckGround()
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
        if(isGrounded)
        {
            jumpCount = maxJumpCount;
            if (isHaveJumpItem)
                jumpCount += 1;
        }    
    }
    private void Movement()
    {
        // Input (유니티에서 입력 관련 클래스)
        // GetAxisRaw : -1 or 0 or 1의 값을 리턴하는 함수.
        // Horizontal(수평) : 왼쪽(-1), 비입력(0), 오른쪽(1)
        // Vertical(수직) : 아래쪽(-1), 비입력(0), 위쪽(1)
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(x, y, 0);

        // transform (Transform 컴포넌트를 참조하고 있다.)
        // Translate(Vector3) : 해당 방향으로 포지션을 변환.
        // deltaTime : 이전 프레임부터 현재 프레임까지 걸린 시간.
        //transform.Translate(direction * 5f * Time.deltaTime);

        // velocity : 현재 속도.
        // x축만 키 제어로 속도가 변한다.
        rigid.velocity = new Vector2(x * moveSpeed, rigid.velocity.y);


        bool isMove = (direction != Vector3.zero);      // 움직이고 있는지?
        anim.SetBool("isMove", isMove);                 // 애니메이터 내부의 파라미터 isMove의 값을 대입.

        // 방향 전환.
        if (x <= -1)
            spriteRenderer.flipX = true;
        else if (x >= 1)
            spriteRenderer.flipX = false;
    }
    private void Jump(bool isForce = false)
    {
        // AddForce(특정 방향으로 힘을 가한다).
        // 위쪽으로 10만큼의 힘을 가한다.
        // ForceMode2D.Force : 민다.
        // ForceMode2D.Impulse : 찬다.
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount > 0)
        {
            jumpCount--;
            isGrounded = false;
            rigid.velocity = new Vector2(rigid.velocity.x, 0f);                 // 현재 내 y축 속도를 0으로 초기화.
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);        // 위 방향으로 jumpPower만큼 힘을 가한다.

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
