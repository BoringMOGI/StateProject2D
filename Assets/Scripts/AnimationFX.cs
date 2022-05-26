using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationFX : MonoBehaviour
{
    public void OnEndEffect()
    {
        Destroy(gameObject);
    }
}
