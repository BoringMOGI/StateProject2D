using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : ItemObject
{
    [SerializeField] AnimationFX eatFx;

    SpriteRenderer spriteRenderer;
    Rigidbody2D rigid;
    ItemData itemData;

    protected override void OnEatItem(PlayerController player)
    {
        Debug.Log("������ ȹ�� : " + itemData.itemName);

        // ������ ȹ�� ����Ʈ ���.
        if (eatFx != null)
            Instantiate(eatFx, transform.position, transform.rotation);

        Destroy(gameObject);
    }
    public void SetupItem(ItemData itemData)
    {
        // ������Ʈ �˻�.
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();

        // ������ ����.
        this.itemData = itemData;
        spriteRenderer.sprite = itemData.itemSprite;
        rigid.AddForce(Vector2.up * 3f, ForceMode2D.Impulse);
    }
}
