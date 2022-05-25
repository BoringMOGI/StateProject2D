using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : ItemObject
{
    [SerializeField] BoxCollider2D boxCollider;
    [SerializeField] Rigidbody2D rigid;

    [Header("Item")]
    [SerializeField] string itemName;

    protected override void OnEatItem(PlayerController player)
    {
        boxCollider.enabled = false;    // �浹ü ����.
        rigid.simulated = false;        // ���� ����X

        Debug.Log("�÷��̾�� ������ ���� : " + itemName);
    }

    public void ShowItem()
    {
        rigid.AddForce(Vector2.up * 3f, ForceMode2D.Impulse);
    }
}
