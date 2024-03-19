using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFactory : MonoBehaviour
{
    private static EnemyFactory m_instance;

    public static EnemyFactory Instance
    {
        get
        {
            if (m_instance == null)
            {
                GameObject gameObject = new GameObject();
                gameObject.name = "Enemy Factory";
                m_instance = gameObject.AddComponent<EnemyFactory>();
            }
            return m_instance;
        }
    }

    public List<GameObject> m_aliveEnemyList;
    public List<GameObject> m_deadEnemyList;
    void Awake()
    {
        if (m_aliveEnemyList == null)
        {
            m_aliveEnemyList = new List<GameObject>();
        }
        if(m_deadEnemyList == null)
        {
            m_deadEnemyList = new List<GameObject>();
        }
    }

    public GameObject SpawnEnemy(GameObject prefab, Vector3 position, EnemyData enemyData, WeaponData weaponData, GameObject patrolPath)
    {
        GameObject newEnemy = Instantiate(prefab, position, Quaternion.identity);
        Enemy enemyRef = newEnemy.GetComponent<Enemy>();
        enemyRef.SetEnemyData(enemyData, patrolPath);
        enemyRef.SetWeaponData(weaponData);
        m_aliveEnemyList.Add(newEnemy);
        
        //projectileRef.onEnemyKilled += RemoveEnemyFromList;
        
        return newEnemy;
    }

    public void RemoveEnemyFromList(GameObject enemy)
    {
        m_deadEnemyList.Add(enemy);
        m_aliveEnemyList.Remove(enemy);
    }

    public void ClearEnemyList()
    {
        if(m_aliveEnemyList.Count > 0)
        {
            foreach (GameObject enemy in m_aliveEnemyList)
            {
                Enemy enemyRef = enemy.GetComponent<Enemy>();
                enemyRef.DamageEnemy();
            }
        }
        
    }

    public int GetEnemyCount()
    {
        return m_aliveEnemyList.Count;
    }

    public void PauseEnemies()
    {
        foreach(GameObject enemy in m_aliveEnemyList)
        {
            Enemy enemyRef = enemy.GetComponent<Enemy>();
            enemyRef.PauseEnemy();
        }
    }

    public void ResumeEnemies()
    {
        foreach (GameObject enemy in m_aliveEnemyList)
        {
            Enemy enemyRef = enemy.GetComponent<Enemy>();
            enemyRef.ResumeEnemy();
        }
    }

    public void ClearAllEnemies()
    {
        foreach(GameObject enemy in m_aliveEnemyList)
        {
            GameObject.Destroy(enemy);
        }
        foreach(GameObject enemy in m_deadEnemyList)
        {
            GameObject.Destroy(enemy);
        }
        m_aliveEnemyList.Clear();
        m_deadEnemyList.Clear();
    }
}
