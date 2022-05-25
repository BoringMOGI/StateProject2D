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
        boxCollider.enabled = false;    // 충돌체 끄기.
        rigid.simulated = false;        // 물리 연산X

        Debug.Log("플레이어에게 아이템 전달 : " + itemName);
    }

    public void ShowItem()
    {
        rigid.AddForce(Vector2.up * 3f, ForceMode2D.Impulse);
    }
}
