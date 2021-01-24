using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

public class IdleState : State<PlayerBase>
{
    private static IdleState _instance;
    public static IdleState Instance
    {
        get
        {
            if (_instance == null)
                _instance = new IdleState();

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
    }
}
