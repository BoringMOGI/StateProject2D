using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// �ش� ������Ʈ�� �¾Ҵٴ� �̺�Ʈ�� �߻���Ų��. (�װ� ���δ�)
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
        // ��ϵ� �ǰ� �̺�Ʈ ȣ��.
        OnDamageEvent?.Invoke(attacker, power);     

        // �ǰ� ������ UI ȣ��.
        if(damageUiPivot != null)
            DamageUIManager.Instance.ShowDamageUI(damageUiPivot.position, power);

        // �̹� ������̶�� ����.
        if(damageFlip != null)
            StopCoroutine(damageFlip);

        // ���ο� �ڷ�ƾ ����.
        damageFlip = StartCoroutine(DamageFlip());

        // �ڷ� �з����� ó��.
        if (movement != null && knockbackDir != Vector2.zero)
            Knockback(attacker);
    }
    private void Knockback(Transform attacker)
    {
        // �ǰ����� ���⿡ ���� ���ư��� ������ �޶�����.
        bool isAttackLeft = attacker.position.x < transform.position.x;

        float x = isAttackLeft ? knockbackDir.x : -knockbackDir.x;
        float y = knockbackDir.y;

        movement.Throw(new Vector2(x, y), knockbackPower);
    }

    IEnumerator DamageFlip()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.5f);      // 0.1�� ����϶�.
        spriteRenderer.color = Color.white;
    }
}
