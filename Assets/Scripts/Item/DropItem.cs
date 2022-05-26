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
        Debug.Log("æ∆¿Ã≈€ »πµÊ : " + itemData.itemName);

        // æ∆¿Ã≈€ »πµÊ ¿Ã∆Â∆Æ ¿Áª˝.
        if (eatFx != null)
            Instantiate(eatFx, transform.position, transform.rotation);

        Destroy(gameObject);
    }
    public void SetupItem(ItemData itemData)
    {
        // ƒƒ∆˜≥Õ∆Æ ∞Àªˆ.
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();

        // æ∆¿Ã≈€ ºº∆√.
        this.itemData = itemData;
        spriteRenderer.sprite = itemData.itemSprite;
        rigid.AddForce(Vector2.up * 3f, ForceMode2D.Impulse);
    }
}
