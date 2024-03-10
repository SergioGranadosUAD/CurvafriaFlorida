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
                gameObject.name = "Projectile Factory";
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

    public GameObject SpawnEnemy(GameObject prefab, Vector3 position, float speed, Quaternion rotation, string instanceTag)
    {
        //GameObject newProjectile = Instantiate(prefab, position, rotation);
        //Projectile projectileRef = newProjectile.GetComponent<Projectile>();
        //projectileRef.SetProjectileData(speed, rotation, instanceTag);
        //m_projectileList.Add(newProjectile);
        //
        //projectileRef.onProjectileDestroyed += RemoveProjectileFromList;
        //
        //return newProjectile;
        return new GameObject();
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
