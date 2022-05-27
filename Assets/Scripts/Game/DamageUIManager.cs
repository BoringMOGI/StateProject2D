using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageUIManager : MonoBehaviour
{
    static DamageUIManager instance;
    public static DamageUIManager Instance => instance;

    [SerializeField] DamageUI prefab;


    private void Awake()
    {
        instance = this;
    }

    public void ShowDamageUI(Vector2 position, int amount)
    {
        DamageUI ui = Instantiate(prefab, transform);
        ui.Setup(position, amount);
    }

}
