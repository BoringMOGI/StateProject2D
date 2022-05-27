using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpBarManager : MonoBehaviour
{
    static HpBarManager instance;
    public static HpBarManager Instance => instance;

    [SerializeField] HpBarUI prefab;
    [SerializeField] int initCount;

    Transform poolStorage;
    Stack<HpBarUI> storage;

    private void Awake()
    {
        instance = this;

        // ���� ������ ������ ��ü ����.
        storage = new Stack<HpBarUI>();

        // Ǯ�� ������Ʈ ������.
        poolStorage = new GameObject("PoolStorage").transform;
        poolStorage.SetParent(transform);
        poolStorage.gameObject.SetActive(false);
        
        // initCount��ŭ �̸� �����.
        for (int i = 0; i < initCount; i++)
        {
            CreatePool();
        }
    }
    private void CreatePool()
    {
        HpBarUI newHpBar = Instantiate(prefab, poolStorage);
        storage.Push(newHpBar);
    }

    public HpBarUI GetHpBar()
    {
        if (storage.Count <= 0)
            CreatePool();

        HpBarUI hpBar = storage.Pop();          // ���� �����Կ��� �ϳ� ������.
        hpBar.transform.SetParent(transform);   // ��Ȱ��ȭ ������Ʈ �ؿ��� Ȱ��ȭ ������Ʈ ������ �ű��.
        return hpBar;
    }
    public void ReturnHpBar(HpBarUI hpBar)
    {
        hpBar.transform.SetParent(poolStorage); // �ݳ��� ������Ʈ�� �ٽ� ��Ȱ��ȭ ������Ʈ ������ �ű��.
        storage.Push(hpBar);                    // ���� �����Կ� �߰��Ѵ�.
    }

}
