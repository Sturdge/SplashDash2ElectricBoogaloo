using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

public class MoveState : State<PlayerBase>
{

    private Vector3 movement;
    private Quaternion targetRotation;

    private static MoveState _instance;
    public static MoveState Instance
    {
        get
        {
            if (_instance == null)
                _instance = new MoveState();

            return _instance;
        }
    }

    public override void EnterState(PlayerBase parent)
    {
    }

    public override void ExitState(PlayerBase parent)
    {
    }

    public override void UpdateState(PlayerBase parent)
    {
        Movement(parent);
        Rotation(parent);
    }

    private void Movement(PlayerBase parent)
    {
        movement = new Vector3
        {
            x = parent.MovementValue.x,
            z = parent.MovementValue.y
        };

        parent.Rigid.MovePosition(parent.transform.position + movement * 5 * Time.fixedDeltaTime);
    }

    private void Rotation(PlayerBase parent)
    {
        if (movement != Vector3.zero)
            targetRotation = Quaternion.LookRotation(movement);

        parent.transform.rotation = targetRotation;
    }
}
