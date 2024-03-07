using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IState
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

    private float m_elapsedTime = 0;

    public void CheckStateConditions()
    {
        if(PlayerRef.Dead)
        {
            StateController.ChangeState("Death");
        }
        if(PlayerRef.Moving)
        {
            StateController.ChangeState("Moving");
        }
    }

    public void OnExecuteState()
    {
        m_elapsedTime += Time.deltaTime;

        if(m_elapsedTime >= PlayerRef.IdleTimer)
        {
            PlayerRef.Animator.ResetTrigger("LongWait");
            PlayerRef.Animator.SetTrigger("LongWait");
            m_elapsedTime = 0;
        }
        CheckStateConditions();
    }

    public void OnExitState()
    {
        
    }

    public void OnStateEnter()
    {
        m_elapsedTime = 0;
    }
}