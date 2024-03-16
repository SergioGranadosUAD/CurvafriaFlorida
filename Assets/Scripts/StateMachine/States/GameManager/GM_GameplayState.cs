using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GM_GameplayState : IState
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
    private GameManager m_gameManagerRef;
    private GameManager GameManagerRef
    {
        get
        {
            if (m_gameManagerRef == null)
            {
                m_gameManagerRef = m_owner.GetComponent<GameManager>();
            }
            return m_gameManagerRef;
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
