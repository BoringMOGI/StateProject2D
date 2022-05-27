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
            //Destroy(gameObject);      // �������°� �ƴ϶� �ٽ� Ǯ�� ���������� �ǵ��ư���.
            HpBarManager.Instance.ReturnHpBar(this);
        }
        else
        {
            // ���� ��ǥ(World)�� ��ũ�� �·�(Screen)�� ��ȯ�Ѵ�.
            // ī�޶� ������ ������ 2D�� �׷����� �����̴�.
            Vector2 screenPoint = cam.WorldToScreenPoint(target.position);
            transform.position = screenPoint;
            UpdateHp(targetStatus.hp, targetStatus.maxHp);
        }
    }

    public void Setup(Transform target, Status targetStatus)
    {
        // ���ʿ� ī�޶� ������ �˻��ؼ� �����Ѵ�.
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
