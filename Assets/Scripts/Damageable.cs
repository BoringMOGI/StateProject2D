using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// �ش� ������Ʈ�� �¾Ҵٴ� �̺�Ʈ�� �߻���Ų��. (�װ� ���δ�)
public class Damageable : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] UnityEvent<Transform, int> OnDamageEvent;

    public void OnDamaged(Transform attacker, int power)
    {
        OnDamageEvent?.Invoke(attacker, power);     // ��ϵ� �ǰ� �̺�Ʈ ȣ��.
        StartCoroutine(DamageFlip());               // �ڷ�ƾ �Լ� ����.
    }
    IEnumerator DamageFlip()
    {
        for(int i = 0; i<3; i++)
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.1f);      // 0.1�� ����϶�.
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
