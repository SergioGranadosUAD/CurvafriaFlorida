using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GM_SetupState : IState
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
    private bool m_loaded = false;

    public void CheckStateConditions()
    {
        if(m_loaded)
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

    }

    public void OnStateEnter()
    {
        GameManagerRef.RestartLevel = false;
        if(GameManagerRef.LevelFinished)
        {
            GameManagerRef.LevelFinished = false;
            Scene newScene = SceneManager.GetSceneByBuildIndex(SceneManager.GetActiveScene().buildIndex + 1);
            if (newScene != null)
            {
                {
                    SceneManager.SetActiveScene(newScene);
                }
            }
            else
            {
                SceneManager.LoadScene(0);
                GameObject.Destroy(GameManagerRef.gameObject);
            }
        }

        if(GameManagerRef.Player != null)
        {
            UIManager.Instance.UnbindUIManagerFromPlayer();
            GameObject.Destroy(GameManagerRef.Player.gameObject);
        }
        EnemyFactory.Instance.ClearAllEnemies();
        ProjectileFactory.Instance.ClearProjectileList();
        PickupFactory.Instance.ClearPickupList();

        GameObject entitySpawnList = GameObject.FindGameObjectWithTag("Spawners");
        foreach(Transform gObject in entitySpawnList.transform)
        {
            ISpawner spawner = gObject.GetComponent<ISpawner>();
            spawner.SpawnEntity();
            gObject.gameObject.SetActive(false);
        }

        GameManagerRef.WinArea.SetActive(false);

        UIManager.Instance.SetupUIManager();
        GameManager.Instance.Player.SetupPlayer();
        m_loaded = true;
    }
}
