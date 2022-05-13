using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayTest : MonoBehaviour
{
    Vector2 hitPoint;       // �浹 ������ ���� ��ġ ����.

    void Update()
    {
        // RaycastHit : ������ �浹�� ��ü�� ������ ����ִ�.
        //RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, 10f);
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, 1f, Vector2.right, 10f);
        if (hit.collider != null)
        {
            hitPoint = hit.point;
            Debug.Log(string.Format("�浹 : {0}", hit.collider.name));
            //Debug.Log($"�浹 : {hit.collider.name}");
        }
        else
            hitPoint = Vector2.zero;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Gizmos.DrawRay(transform.position, Vector2.right * 10f);
        if (hitPoint == Vector2.zero)
            Gizmos.DrawLine(transform.position, transform.position + (Vector3.right * 10f));
        else
        {
            Gizmos.DrawLine(transform.position, hitPoint);
            Gizmos.DrawWireSphere(hitPoint, 0.2f);
        }
    }
}
