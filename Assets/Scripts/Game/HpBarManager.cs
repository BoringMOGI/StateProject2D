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

        // 스택 구조의 보관함 객체 생성.
        storage = new Stack<HpBarUI>();

        // 풀링 오브젝트 보관함.
        poolStorage = new GameObject("PoolStorage").transform;
        poolStorage.SetParent(transform);
        poolStorage.gameObject.SetActive(false);
        
        // initCount만큼 미리 만들기.
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

        HpBarUI hpBar = storage.Pop();          // 스택 보관함에서 하나 꺼낸다.
        hpBar.transform.SetParent(transform);   // 비활성화 오브젝트 밑에서 활성화 오브젝트 밑으로 옮긴다.
        return hpBar;
    }
    public void ReturnHpBar(HpBarUI hpBar)
    {
        hpBar.transform.SetParent(poolStorage); // 반납한 오브젝트를 다시 비활성화 오브젝트 밑으로 옮긴다.
        storage.Push(hpBar);                    // 스택 보관함에 추가한다.
    }

}
