using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] Movement2D movement;

    bool isAttack;      // 공격중인가?

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z) && !isAttack)
        {
            OnStartAttack();
        }
    }

    public void OnStartAttack()
    {
        anim.SetTrigger("onAttack");
        movement.OnLockMovment(true);
        isAttack = true;
    }
    public void OnEndAttack()
    {
        Debug.Log("OnEndAttack");
        movement.OnLockMovment(false);
        isAttack = false;
    }
}
