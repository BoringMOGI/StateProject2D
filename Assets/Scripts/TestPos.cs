using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPos : MonoBehaviour
{
    public Rigidbody2D rigid;
    public GameObject prefab;
    public Transform bulletPivot;

    public float moveSpeed;
    public float rotateSpeed;

    float addForce;

    private void Update()
    {
        //float x = Input.GetAxis("Horizontal");
        //float y = Input.GetAxis("Vertical");

        // x,y�Է� ���� ���� ����(����)�� �ӵ�(5)�� Time.dltaTime�� ���Ѵ�.
        //transform.position += new Vector3(x, y, 0) * Time.deltaTime * 5f;
        //rigid.velocity = new Vector2(x, y) * moveSpeed;

        // Space�ٸ� ������ �ִ� ���� addForce�� ���� ���Ѵ�.
        if(Input.GetKey(KeyCode.Space))
        {
            addForce += Time.deltaTime * 5;
        }
        // �����̽��ٸ� ���� �� �������� �����ϰ� '�� ����' ������ �������� ����ŭ �߻��Ѵ�.
        if(Input.GetKeyUp(KeyCode.Space))
        {
            GameObject bullet = Instantiate(prefab, bulletPivot.position, bulletPivot.rotation);
            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
            rigid.AddForce(transform.right * (10f + addForce), ForceMode2D.Impulse);
            addForce = 0.0f;
        }

        if(Input.GetKey(KeyCode.UpArrow))
        {
            // Vector3.forward(0,0,1) z�� �������� rotateSpeed��ŭ ȸ�����Ѷ�.
            transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);
        }
        else if(Input.GetKey(KeyCode.DownArrow))
        {
            transform.Rotate(Vector3.forward * -rotateSpeed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        string tag = collision.gameObject.tag;

        switch(tag)
        {
            case "Ghost":
                Destroy(gameObject);
                break;

            case "Coin":
                Destroy(collision.gameObject);
                break;

            default:
                break;
        }
        
    }
}