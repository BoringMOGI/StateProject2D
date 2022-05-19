using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// 해당 컴포넌트는 맞았다는 이벤트를 발생시킨다. (그게 전부다)
public class Damageable : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] UnityEvent<Transform, int> OnDamageEvent;

    public void OnDamaged(Transform attacker, int power)
    {
        OnDamageEvent?.Invoke(attacker, power);     // 등록된 피격 이벤트 호출.
        StartCoroutine(DamageFlip());               // 코루틴 함수 실행.
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
