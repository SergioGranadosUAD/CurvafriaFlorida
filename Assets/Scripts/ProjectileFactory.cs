using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileFactory : MonoBehaviour
{
    private static ProjectileFactory m_instance;

    public static ProjectileFactory Instance
    {
        get
        {
            if (m_instance == null)
            {
                GameObject gameObject = new GameObject();
                gameObject.name = "Projectile Factory";
                m_instance = gameObject.AddComponent<ProjectileFactory>();
            }
            return m_instance;
        }
    }

    public List<GameObject> m_projectileList;
    void Awake()
    {
        if (m_projectileList == null)
        {
            m_projectileList = new List<GameObject>();
        }
    }

    public GameObject SpawnProjectile(GameObject prefab, Vector3 position, float speed, Quaternion rotation, string instanceTag)
    {
        GameObject newProjectile = Instantiate(prefab, position, rotation);
        Projectile projectileRef = newProjectile.GetComponent<Projectile>();
        projectileRef.SetProjectileData(speed, rotation, instanceTag);
        m_projectileList.Add(newProjectile);

        projectileRef.onProjectileDestroyed += RemoveProjectileFromList;

        return newProjectile;
    }

    private void RemoveProjectileFromList(GameObject projectile)
    {
        m_projectileList.Remove(projectile);
    }

    public void ClearProjectileList()
    {
        if (m_projectileList.Count > 0)
        {
            foreach (GameObject projectile in m_projectileList)
            {
                Destroy(projectile);
            }
            m_projectileList.Clear();
        }
    }
}
