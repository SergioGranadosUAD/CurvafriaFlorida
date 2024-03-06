using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//Generic finite state machine, can be used by any GameObject and custom states can be added to its internal list for execution
public class StateMachine
{
    private GameObject m_owner;
    public GameObject Owner
    {
        get { return m_owner; }
        set { m_owner = value; }
    }

    private IState m_currentState;
    public IState CurrentState
    {
        get { return m_currentState; }
        set { m_currentState = value; }
    }

    private Dictionary<string, IState> m_states = new Dictionary<string, IState>();

    //Add a state to the list of states managed by the state machine
    public void AddState(IState state, string stateName)
    {
        state.StateController = this;
        state.Owner = m_owner;
        m_states.Add(stateName, state);
    }

    //Change the current state for a new one, based on the name given by the parameter
    public void ChangeState(string stateName)
    {
        if(m_currentState != null)
        {
            m_currentState.OnExitState();
        }
        m_states.TryGetValue(stateName, out m_currentState);
        m_currentState.OnStateEnter();
    }

}
