using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// AudioSE�� ������Ʈ�� ����ϱ� ���ؼ��� AudioSource�� �پ��־�� �Ѵ�.
[RequireComponent(typeof(AudioSource))]
public class AudioSE : MonoBehaviour
{
    AudioSource source;

    public void PlaySE(AudioClip clip)
    {
        if(source == null)
            source = GetComponent<AudioSource>();

        source.clip = clip;
        source.Play();
    }

    private void Update()
    {
        if (source.isPlaying == false)
            AudioManager.Instance.ReturnPool(this);
    }
}
