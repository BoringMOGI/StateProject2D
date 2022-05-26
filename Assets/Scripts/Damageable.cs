using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// 해당 컴포넌트는 맞았다는 이벤트를 발생시킨다. (그게 전부다)
public class Damageable : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Movement2D movement;
    [SerializeField] Transform damageUiPivot;
    [SerializeField] Vector2 knockbackDir;
    [SerializeField] float knockbackPower;

    [SerializeField] UnityEvent<Transform, int> OnDamageEvent;

    Coroutine damageFlip;

    public void OnDamaged(Transform attacker, int power)
    {
        // 등록된 피격 이벤트 호출.
        OnDamageEvent?.Invoke(attacker, power);     

        // 피격 데미지 UI 호출.
        if(damageUiPivot != null)
            DamageUIManager.Instance.ShowDamageUI(damageUiPivot.position, power);

        // 이미 재생중이라면 중지.
        if(damageFlip != null)
            StopCoroutine(damageFlip);

        // 새로운 코루틴 실행.
        damageFlip = StartCoroutine(DamageFlip());

        // 뒤로 밀려나는 처리.
        if (movement != null && knockbackDir != Vector2.zero)
            Knockback(attacker);
    }
    private void Knockback(Transform attacker)
    {
        // 피격자의 방향에 따라서 날아가는 방향이 달라진다.
        bool isAttackLeft = attacker.position.x < transform.position.x;

        float x = isAttackLeft ? knockbackDir.x : -knockbackDir.x;
        float y = knockbackDir.y;

        movement.Throw(new Vector2(x, y), knockbackPower);
    }

    IEnumerator DamageFlip()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.5f);      // 0.1초 대기하라.
        spriteRenderer.color = Color.white;
    }
}
