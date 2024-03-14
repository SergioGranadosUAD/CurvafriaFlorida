using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_PatrolState : IState
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

    private List<Vector3> m_patrolNodes = new List<Vector3>();
    private int m_currentNode = 0;

    public void CheckStateConditions()
    {
        if (EnemyRef.Dead)
        {
            StateController.ChangeState("Death");
        }
        if (EnemyRef.PlayerDetected)
        {
            StateController.ChangeState("Chase");
        }
    }

    public void OnExecuteState()
    {
        if(Vector3.Distance(EnemyRef.Position, m_patrolNodes[m_currentNode]) > 0)
        {
            EnemyRef.NavAgent.destination = m_patrolNodes[m_currentNode];
        }else
        {
            m_currentNode++;
        }

        CheckStateConditions();
    }

    public void OnExitState()
    {

    }

    public void OnStateEnter()
    {
        foreach(Transform child in EnemyRef.PatrolPath.transform)
        {
            m_patrolNodes.Add(child.position);
        }
    }
}
