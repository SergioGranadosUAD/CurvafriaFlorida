using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GM_PauseState : IState
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
        if(!GameManagerRef.Paused)
        {
            StateController.ChangeState("Gameplay");
        }
    }

    public void OnExecuteState()
    {

        CheckStateConditions();
    }

    public void OnExitState()
    {
        //Continua con la actualizaci�n de todos las entidades y oculta el men�.
        UIManager.Instance.ShowPauseMenu(false);
        EnemyFactory.Instance.ResumeEnemies();
        ProjectileFactory.Instance.ResumeProjectiles();
        GameManagerRef.Player.ResumePlayer();
    }

    public void OnStateEnter()
    {
        //Pausa la actualizaci�n de todas las entidades y muestra el men�.
        UIManager.Instance.ShowPauseMenu(true);
        EnemyFactory.Instance.PauseEnemies();
        ProjectileFactory.Instance.PauseProjectiles();
        GameManagerRef.Player.PausePlayer();
    }
}
