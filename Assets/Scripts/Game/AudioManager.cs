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

        // Ǯ�� ������Ʈ�� ���� ��ġ.
        storageParent = new GameObject("storage").transform;
        storageParent.SetParent(transform);
        storageParent.gameObject.SetActive(false);

        // ���� ��ü ����.
        storageStack = new Stack<AudioSE>();
    }
    private void Start()
    {
        for (int i = 0; i < createCount; i++)
            CreatePool();
    }

    private void CreatePool()
    {
        // storageParent������Ʈ�� ������ prefab�� clone���� �����Ѵ�.
        AudioSE newSE = Instantiate(prefab, storageParent);
        storageStack.Push(newSE);
    }
    private AudioSE GetPool()
    {
        // ���� ������ ������ 0���϶��
        if (storageStack.Count <= 0)
            CreatePool();

        // ���ÿ��� SE�� ������ �θ� ������Ʈ�� ���� ������ �ڿ� ��ȯ.
        AudioSE se = storageStack.Pop();
        se.transform.SetParent(transform);
        return se;
    }
    public void ReturnPool(AudioSE se)
    {
        // �ٽ� ��Ȱ��ȭ ������Ʈ ������ �ΰ� ���ÿ� �ִ´�.
        se.transform.SetParent(storageParent);
        storageStack.Push(se);
    }

    public void PlaySE(AudioClip clip)
    {
        AudioSE audioSE = GetPool();
        audioSE.PlaySE(clip);
    }

}
