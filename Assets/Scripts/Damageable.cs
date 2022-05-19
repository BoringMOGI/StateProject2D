using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// 해당 컴포넌트를 추가하기 위해서는 Collider2D가 강제된다.
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Status))]
public class Damageable : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Status stat;
    
    [Header("Throw")]
    [SerializeField] Movement2D movement2D;         // 이동 관련 클래스.
    [SerializeField] Vector2 throwDir;              // 피격시 날아가는 방향.
    [SerializeField] float throwPower;              // 피격시 날아가는 힘.

    [Header("Event")]
    [SerializeField] UnityEvent OnDamageEvent;
    [SerializeField] UnityEvent OnDeadEvent;

    //[SerializeField] UnityEvent<Transform, Status> OnDamagedEvent;

    public bool isAlive => stat.hp > 0;

    public void OnDamaged(Transform attacker, int power)
    {
        // 죽었다면 더 이상 맞지 않는다.
        if (!isAlive)
        {
            Debug.Log("플레이은 이미 체력이 0이다");
            return;
        }

        // Mathf.Clamp(값, 최소값, 최대값)
        //  => 값을 최소~최대의 사이 값으로 조정.
        stat.hp = Mathf.Clamp(stat.hp - power, 0, stat.maxHp);

        OnDamageEvent?.Invoke();                    // 이벤트 호출.
        movement2D.Throw(throwDir, throwPower);     // 피격시 날아감.

        if(isAlive)
        {
            StartCoroutine(DamageFlip());    // 코루틴 함수 실행.
        }
        else
        {
            OnDeadEvent?.Invoke();           // 죽는 이벤트 발생.
        }
    }
    IEnumerator DamageFlip()
    {
        for(int i = 0; i<3; i++)
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.1f);      // 0.1초 대기하라.
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
