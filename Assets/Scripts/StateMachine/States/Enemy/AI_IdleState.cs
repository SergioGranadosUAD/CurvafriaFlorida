using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_IdleState : IState
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
    private Enemy m_enemyRef;
    private Enemy EnemyRef
    {
        get
        {
            if (m_enemyRef == null)
            {
                m_enemyRef = m_owner.GetComponent<Enemy>();
            }
            return m_enemyRef;
        }
    }

    private float m_elapsedTime = 0;

    public void CheckStateConditions()
    {
        if (EnemyRef.Dead)
        {
            StateController.ChangeState("Death");
        }
        if(EnemyRef.PlayerDetected)
        {
            StateController.ChangeState("Chase");
        }
    }

    public void OnExecuteState()
    {
        m_elapsedTime += Time.deltaTime;

        if (m_elapsedTime >= EnemyRef.IdleTimer)
        {
            EnemyRef.Animator.ResetTrigger("LongWait");
            EnemyRef.Animator.SetTrigger("LongWait");
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
