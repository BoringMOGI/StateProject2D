using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;       // �̺�Ʈ ���� Ŭ����.

public abstract class ItemObject : MonoBehaviour
{
    [SerializeField] Animator anim;

    [Header("Event")]
    [SerializeField] UnityEvent OnEatItemEvent;

    // �ݶ��̴��� �浹ü�� �浹���� ��.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        OnTriggerEnter2D(collision.collider);
    }

    // Ʈ���Ű� �浹ü�� �浹������ �� �� �Ҹ��� �̺�Ʈ �Լ�.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �浹�� ������Ʈ���Լ� Ư�� Component�� �˻��Ѵ�.
        // �˻��� �������̾����� ���η� �����ؼ� �Լ� ȣ��.
        PlayerController player = collision.GetComponent<PlayerController>();
        if(player != null)
        {
            // �̺�Ʈ ȣ��.
            anim.SetTrigger("onEat");
            OnEatItemEvent?.Invoke();       // ��� �̺�Ʈ ȣ��.
            OnEatItem(player);              // ������ �Ա� �Լ� ȣ��.
        }
    }

    private void OnEndEffect()
    {
        Destroy(gameObject);
    }
    protected abstract void OnEatItem(PlayerController player);
}
