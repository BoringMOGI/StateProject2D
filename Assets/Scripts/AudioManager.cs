using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    static AudioManager instance;
    public static AudioManager Instance => instance;

    [SerializeField] AudioSE prefab;
    [SerializeField] int createCount;

    Transform storageParent;
    Stack<AudioSE> storageStack;

    private void Awake()
    {
        instance = this;

        // 풀링 오브젝트의 저장 위치.
        storageParent = new GameObject("storage").transform;
        storageParent.SetParent(transform);
        storageParent.gameObject.SetActive(false);

        // 스택 객체 생성.
        storageStack = new Stack<AudioSE>();
    }
    private void Start()
    {
        for (int i = 0; i < createCount; i++)
            CreatePool();
    }

    private void CreatePool()
    {
        // storageParent오브젝트의 하위에 prefab을 clone으로 생성한다.
        AudioSE newSE = Instantiate(prefab, storageParent);
        storageStack.Push(newSE);
    }
    private AudioSE GetPool()
    {
        // 만약 스택의 개수가 0이하라면
        if (storageStack.Count <= 0)
            CreatePool();

        // 스택에서 SE를 꺼내고 부모 오브젝트를 나로 변경한 뒤에 반환.
        AudioSE se = storageStack.Pop();
        se.transform.SetParent(transform);
        return se;
    }
    public void ReturnPool(AudioSE se)
    {
        // 다시 비활성화 오브젝트 하위에 두고 스택에 넣는다.
        se.transform.SetParent(storageParent);
        storageStack.Push(se);
    }

    public void PlaySE(AudioClip clip)
    {
        AudioSE audioSE = GetPool();
        audioSE.PlaySE(clip);
    }

}
