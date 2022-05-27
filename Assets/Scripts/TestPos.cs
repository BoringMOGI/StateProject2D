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

        // x,y입력 값에 따른 방향(벡터)에 속도(5)와 Time.dltaTime을 곱한다.
        //transform.position += new Vector3(x, y, 0) * Time.deltaTime * 5f;
        //rigid.velocity = new Vector2(x, y) * moveSpeed;

        // Space바를 누르고 있는 동안 addForce에 값을 더한다.
        if(Input.GetKey(KeyCode.Space))
        {
            addForce += Time.deltaTime * 5;
        }
        // 스페이스바를 땠을 때 프리팹을 생성하고 '내 기준' 오른쪽 방향으로 힘만큼 발사한다.
        if(Input.GetKeyUp(KeyCode.Space))
        {
            GameObject bullet = Instantiate(prefab, bulletPivot.position, bulletPivot.rotation);
            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
            rigid.AddForce(transform.right * (10f + addForce), ForceMode2D.Impulse);
            addForce = 0.0f;
        }

        if(Input.GetKey(KeyCode.UpArrow))
        {
            // Vector3.forward(0,0,1) z축 방향으로 rotateSpeed만큼 회전시켜라.
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