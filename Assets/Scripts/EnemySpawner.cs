using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum EnemyType
{
    MELEE,
    PISTOL,
    RIFLE,
    SHOTGUN
}
public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemyType enemyType = EnemyType.MELEE;
    [SerializeField] private GameObject patrolNodes;
    private GameObject m_enemyPrefab;
    private EnemyData m_enemyData;
    private WeaponData m_weaponData;
    // Start is called before the first frame update
    void Start()
    {
        switch(enemyType)
        {
            case EnemyType.MELEE:
                m_enemyPrefab = Resources.Load<GameObject>("Prefabs/Enemies/EnemyHoodedBandit");
                m_enemyData = Resources.Load<EnemyData>("ScriptableObjects/Enemies/MeleeEnemy");
                m_weaponData = Resources.Load<WeaponData>("ScriptableObjects/Weapons/Melee");
                break;
            case EnemyType.PISTOL:
                m_enemyPrefab = Resources.Load<GameObject>("Prefabs/Enemies/EnemySmuggler");
                m_enemyData = Resources.Load<EnemyData>("ScriptableObjects/Enemies/PistolEnemy");
                m_weaponData = Resources.Load<WeaponData>("ScriptableObjects/Weapons/Pistol");
                break;
            case EnemyType.RIFLE:
                m_enemyPrefab = Resources.Load<GameObject>("Prefabs/Enemies/EnemyBandit");
                m_enemyData = Resources.Load<EnemyData>("ScriptableObjects/Enemies/RifleEnemy");
                m_weaponData = Resources.Load<WeaponData>("ScriptableObjects/Weapons/Rifle");
                break;
            case EnemyType.SHOTGUN:
                m_enemyPrefab = Resources.Load<GameObject>("Prefabs/Enemies/EnemyMaskedBandit");
                m_enemyData = Resources.Load<EnemyData>("ScriptableObjects/Enemies/ShotgunEnemy");
                m_weaponData = Resources.Load<WeaponData>("ScriptableObjects/Weapons/Shotgun");
                break;
        }
        EnemyFactory.Instance.SpawnEnemy(m_enemyPrefab, transform.position, m_enemyData, m_weaponData, patrolNodes);

        GameObject.Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
