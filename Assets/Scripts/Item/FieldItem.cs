using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldItem : ItemObject
{
    [SerializeField] int amount;
    [SerializeField] BoxCollider2D boxCollider;

    protected override void OnEatItem(PlayerController player)
    {
        boxCollider.enabled = false;
        player.GetGem(amount);
    }
}
