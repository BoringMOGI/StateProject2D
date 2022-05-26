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

    float countdown;        // ������� �ð�.

    private void Update()
    {
        // ���� ��ǥ�� ��ũ�� ��ǥ�� ��ȯ.
        transform.position = cam.WorldToScreenPoint(position);
        if ((countdown -= Time.deltaTime) <= 0.0f)
            Destroy(gameObject);
    }

    public void Setup(Vector2 position, int amount)
    {
        this.position = position;

        cam = Camera.main;      // ���� ī�޶� ĳ��.
        countdown = showTime;
        damageText.text = string.Format("-{0}", amount);
    }

}
