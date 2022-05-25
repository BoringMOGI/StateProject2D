using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// AudioSE를 컴포넌트로 사용하기 위해서는 AudioSource가 붙어있어야 한다.
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
