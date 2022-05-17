using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �ش� ������Ʈ�� �߰��ϱ� ���ؼ��� Collider2D�� �����ȴ�.
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Status))]
public class Damageable : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Status stat;

    public bool isAlive => stat.hp > 0;

    public void OnDamaged(Transform attacker, int power)
    {
        // Mathf.Clamp(��, �ּҰ�, �ִ밪)
        //  => ���� �ּ�~�ִ��� ���� ������ ����.
        stat.hp = Mathf.Clamp(stat.hp - power, 0, stat.maxHp);
        StartCoroutine(DamageFlip());                   // �ڷ�ƾ �Լ� ����.
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
