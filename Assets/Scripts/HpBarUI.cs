using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBarUI : MonoBehaviour
{
    [SerializeField] Image hpImage;
    [SerializeField] Transform target;
    [SerializeField] Camera cam;

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
        }
        else
        {
            // 월드 좌표(World)를 스크린 좌료(Screen)로 변환한다.
            // 카메라에 투과된 세상이 2D로 그려지기 때문이다.
            Vector2 screenPoint = cam.WorldToScreenPoint(target.position);
            transform.position = screenPoint;
        }
    }

    public void UpdateHp(float current, float max)
    {
        hpImage.fillAmount = current / max;
    }
}
