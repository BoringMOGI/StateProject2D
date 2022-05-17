using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 해당 컴포넌트를 추가하기 위해서는 Collider2D가 강제된다.
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Status))]
public class Damageable : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Status stat;

    public bool isAlive => stat.hp > 0;

    public void OnDamaged(Transform attacker, int power)
    {
        // Mathf.Clamp(값, 최소값, 최대값)
        //  => 값을 최소~최대의 사이 값으로 조정.
        stat.hp = Mathf.Clamp(stat.hp - power, 0, stat.maxHp);
        StartCoroutine(DamageFlip());                   // 코루틴 함수 실행.
    }
    IEnumerator DamageFlip()
    {
        for(int i = 0; i<3; i++)
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.1f);      // 0.1초 대기하라.
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
