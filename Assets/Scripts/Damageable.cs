using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �ش� ������Ʈ�� �߰��ϱ� ���ؼ��� Collider2D�� �����ȴ�.
[RequireComponent(typeof(Collider2D))]
public class Damageable : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] int maxHp;

    private int hp;

    public bool isAlive => hp > 0;

    private void Start()
    {
        hp = maxHp;
    }

    public void OnDamaged(Transform attacker, int power)
    {
        // Mathf.Clamp(��, �ּҰ�, �ִ밪)
        //  => ���� �ּ�~�ִ��� ���� ������ ����.
        hp = Mathf.Clamp(hp - power, 0, maxHp);
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
