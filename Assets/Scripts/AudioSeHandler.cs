using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSeHandler : MonoBehaviour
{
    public void PlaySE(AudioClip clip)
    {
        AudioManager.Instance.PlaySE(clip);
    }
}
