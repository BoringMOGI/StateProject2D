using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] Collider2D collider2D;
    [SerializeField] int amount;

    // 트리거가 충돌체랑 충돌했을때 한 번 불리는 이벤트 함수.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 충돌한 오브젝트에게서 특정 Component를 검색한다.
        // 검색이 성공적이었으면 내부로 접근해서 함수 호출.
        PlayerController player = collision.GetComponent<PlayerController>();
        if(player != null)
        {
            player.GetGem(amount);     // 플레이어에게 골드 전달.
            anim.SetTrigger("onEat");   // 애니메이션 호출.
            collider2D.enabled = false; // 출돌체 끄기.
        }
    }
    // 트리거가 충돌체랑 충돌하는 동안 계속 불리는 이벤트 함수.
    private void OnTriggerStay2D(Collider2D collision)
    {
    }
    // 트리거가 충돌체랑 떨어졌을 때 한 번 불리는 이벤트 함수.
    private void OnTriggerExit2D(Collider2D collision)
    {
    }

    public void OnEndEffect()
    {
        Destroy(gameObject);
    }
}
