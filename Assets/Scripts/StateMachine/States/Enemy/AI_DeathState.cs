using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_DeathState : IState
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
        EnemyRef.NavAgent.enabled = false;
        Debug.Log("Enemy dead");
    }
}