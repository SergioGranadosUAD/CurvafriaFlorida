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

    public List<GameObject> m_enemyList;
    void Awake()
    {
        if (m_enemyList == null)
        {
            m_enemyList = new List<GameObject>();
        }
    }

    public GameObject SpawnEnemy(GameObject prefab, Vector3 position, EnemyData enemyData, WeaponData weaponData, GameObject patrolPath)
    {
        GameObject newEnemy = Instantiate(prefab, position, Quaternion.identity);
        Enemy enemyRef = newEnemy.GetComponent<Enemy>();
        enemyRef.SetEnemyData(enemyData, patrolPath);
        enemyRef.SetWeaponData(weaponData);
        m_enemyList.Add(newEnemy);
        
        //projectileRef.onEnemyKilled += RemoveEnemyFromList;
        
        return newEnemy;
    }

    private void RemoveEnemyFromList(GameObject enemy)
    {
        m_enemyList.Remove(enemy);
    }

    public void ClearEnemyList()
    {
        if(m_enemyList.Count > 0)
        {
            foreach (GameObject enemy in m_enemyList)
            {
                Destroy(enemy);
            }
            m_enemyList.Clear();
        }
        
    }
}
