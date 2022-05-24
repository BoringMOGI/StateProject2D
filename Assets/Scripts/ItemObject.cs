using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [SerializeField] int amount;

    // Ʈ���Ű� �浹ü�� �浹������ �� �� �Ҹ��� �̺�Ʈ �Լ�.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �浹�� ������Ʈ���Լ� Ư�� Component�� �˻��Ѵ�.
        // �˻��� �������̾����� ���η� �����ؼ� �Լ� ȣ��.
        PlayerController player = collision.GetComponent<PlayerController>();
        if(player != null)
        {
            player.GetGold(amount);
            Destroy(gameObject);
        }
    }
    // Ʈ���Ű� �浹ü�� �浹�ϴ� ���� ��� �Ҹ��� �̺�Ʈ �Լ�.
    private void OnTriggerStay2D(Collider2D collision)
    {
    }
    // Ʈ���Ű� �浹ü�� �������� �� �� �� �Ҹ��� �̺�Ʈ �Լ�.
    private void OnTriggerExit2D(Collider2D collision)
    {
    }
}
