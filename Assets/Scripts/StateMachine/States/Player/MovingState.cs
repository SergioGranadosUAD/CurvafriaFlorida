using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingState : IState
{
    private StateMachine m_stateController;
    public StateMachine StateController
    {
        get { return m_stateController; }
        set { m_stateController = value; }
    }
    private GameObject m_owner;
    public GameObject Owner
    {
        get { return m_owner; }
        set { m_owner = value; }
    }
    private Player m_playerRef;
    private Player PlayerRef
    {
        get
        {
            if (m_playerRef == null)
            {
                m_playerRef = m_owner.GetComponent<Player>();
            }
            return m_playerRef;
        }
    }

    public void CheckStateConditions()
    {

    }

    public void OnExecuteState()
    {
        CheckStateConditions();
    }

    public void OnExitState()
    {

    }

    public void OnStateEnter()
    {

    }
}