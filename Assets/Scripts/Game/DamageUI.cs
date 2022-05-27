using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DamageUI : MonoBehaviour
{
    [SerializeField] Text damageText;
    [SerializeField] float showTime;

    Camera cam;
    Vector2 position;

    float countdown;        // 사라지는 시간.

    private void Update()
    {
        // 월드 좌표를 스크린 좌표로 변환.
        transform.position = cam.WorldToScreenPoint(position);
        if ((countdown -= Time.deltaTime) <= 0.0f)
            Destroy(gameObject);
    }

    public void Setup(Vector2 position, int amount)
    {
        this.position = position;

        cam = Camera.main;      // 메인 카메라 캐싱.
        countdown = showTime;
        damageText.text = string.Format("-{0}", amount);
    }

}
