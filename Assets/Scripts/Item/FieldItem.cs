using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldItem : ItemObject
{
    public enum FIELD_ITEM
    {
        GEM,        // ∫∏ºÆ.
    }

    [SerializeField] ItemData itemData;
    [SerializeField] FIELD_ITEM itemType;

    private void Start()
    {
        
    }

    protected override void OnEatItem(PlayerController player)
    {
        Debug.Log("æ∆¿Ã≈€ »πµÊ : " + itemData.itemName);
        Destroy(gameObject);
    }
}
