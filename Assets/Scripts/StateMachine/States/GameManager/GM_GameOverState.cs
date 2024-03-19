using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GM_GameOverState : IState
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
        if(GameManagerRef.RestartLevel)
        {
            StateController.ChangeState("SetupLevel");
        }
    }

    public void OnExecuteState()
    {
        //Al presionar cualquier tecla se reinicia el nivel
        if(Keyboard.current.anyKey.isPressed)
        {
            GameManagerRef.RestartLevel = true;
        }

        CheckStateConditions();
    }

    public void OnExitState()
    {
        //Al salir oculta el menú.
        UIManager.Instance.ShowGameOverMenu(false);
    }

    public void OnStateEnter()
    {
        //Al entrar muestra la pantalla de muerte y desactiva los controles del jugador.
        UIManager.Instance.ShowGameOverMenu(true);
        GameManager.Instance.Player.IAPlayer.BasicMovement.Disable();
    }
}