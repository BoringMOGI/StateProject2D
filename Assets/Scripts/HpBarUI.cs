using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBarUI : MonoBehaviour
{
    [SerializeField] Image hpImage;

    Camera cam;
    Transform target;
    Status targetStatus;

    void Update()
    {
        if (targetStatus.hp <= 0)
        {
            //Destroy(gameObject);      // 지워지는게 아니라 다시 풀링 보관함으로 되돌아간다.
            HpBarManager.Instance.ReturnHpBar(this);
        }
        else
        {
            // 월드 좌표(World)를 스크린 좌료(Screen)로 변환한다.
            // 카메라에 투과된 세상이 2D로 그려지기 때문이다.
            Vector2 screenPoint = cam.WorldToScreenPoint(target.position);
            transform.position = screenPoint;
            UpdateHp(targetStatus.hp, targetStatus.maxHp);
        }
    }

    public void Setup(Transform target, Status targetStatus)
    {
        // 최초에 카메라가 없으면 검색해서 대입한다.
        if (cam == null)
            cam = Camera.main;

        this.target = target;
        this.targetStatus = targetStatus;
    }

    private void UpdateHp(float current, float max)
    {
        hpImage.fillAmount = current / max;
    }
}
