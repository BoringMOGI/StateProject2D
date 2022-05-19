using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// �ش� ������Ʈ�� �߰��ϱ� ���ؼ��� Collider2D�� �����ȴ�.
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Status))]
public class Damageable : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Status stat;
    
    [Header("Throw")]
    [SerializeField] Movement2D movement2D;         // �̵� ���� Ŭ����.
    [SerializeField] Vector2 throwDir;              // �ǰݽ� ���ư��� ����.
    [SerializeField] float throwPower;              // �ǰݽ� ���ư��� ��.

    [Header("Event")]
    [SerializeField] UnityEvent OnDamageEvent;
    [SerializeField] UnityEvent OnDeadEvent;

    //[SerializeField] UnityEvent<Transform, Status> OnDamagedEvent;

    public bool isAlive => stat.hp > 0;

    public void OnDamaged(Transform attacker, int power)
    {
        // �׾��ٸ� �� �̻� ���� �ʴ´�.
        if (!isAlive)
        {
            Debug.Log("�÷����� �̹� ü���� 0�̴�");
            return;
        }

        // Mathf.Clamp(��, �ּҰ�, �ִ밪)
        //  => ���� �ּ�~�ִ��� ���� ������ ����.
        stat.hp = Mathf.Clamp(stat.hp - power, 0, stat.maxHp);

        OnDamageEvent?.Invoke();                    // �̺�Ʈ ȣ��.
        movement2D.Throw(throwDir, throwPower);     // �ǰݽ� ���ư�.

        if(isAlive)
        {
            StartCoroutine(DamageFlip());    // �ڷ�ƾ �Լ� ����.
        }
        else
        {
            OnDeadEvent?.Invoke();           // �״� �̺�Ʈ �߻�.
        }
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
