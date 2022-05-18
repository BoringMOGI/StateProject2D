using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : CharactorController
{

    private new void Start()
    {
        base.Start();
    }

    private void Update()
    {
        if (isAttack == false)
        {
            Movement();
        }
    }

    void Movement()
    {
        inputX = (movement.moveDirection == VECTOR.Left) ? -1 : 1;
        movement.Move(inputX);
    }
}
