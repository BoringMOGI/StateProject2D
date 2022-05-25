using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;       // 이벤트 관련 클래스.

public abstract class ItemObject : MonoBehaviour
{
    [SerializeField] Animator anim;

    [Header("Event")]
    [SerializeField] UnityEvent OnEatItemEvent;

    // 콜라이더가 충돌체랑 충돌했을 때.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        OnTriggerEnter2D(collision.collider);
    }

    // 트리거가 충돌체랑 충돌했을때 한 번 불리는 이벤트 함수.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 충돌한 오브젝트에게서 특정 Component를 검색한다.
        // 검색이 성공적이었으면 내부로 접근해서 함수 호출.
        PlayerController player = collision.GetComponent<PlayerController>();
        if(player != null)
        {
            // 이벤트 호출.
            anim.SetTrigger("onEat");
            OnEatItemEvent?.Invoke();       // 등록 이벤트 호출.
            OnEatItem(player);              // 아이템 먹기 함수 호출.
        }
    }

    private void OnEndEffect()
    {
        Destroy(gameObject);
    }
    protected abstract void OnEatItem(PlayerController player);
}
