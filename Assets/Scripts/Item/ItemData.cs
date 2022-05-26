using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "GameData/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;         // 아이템 이름
    public string description;      // 내용
    public Sprite itemSprite;       // 아이템 이미지.
}
