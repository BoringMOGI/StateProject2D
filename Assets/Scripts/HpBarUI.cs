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
            // ���� ��ǥ(World)�� ��ũ�� �·�(Screen)�� ��ȯ�Ѵ�.
            // ī�޶� ������ ������ 2D�� �׷����� �����̴�.
            Vector2 screenPoint = cam.WorldToScreenPoint(target.position);
            transform.position = screenPoint;
        }
    }

    public void UpdateHp(float current, float max)
    {
        hpImage.fillAmount = current / max;
    }
}
