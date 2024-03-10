using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : MonoBehaviour, IWeapon
{
    GameObject m_weaponRoot;
    public GameObject WeaponRoot { get { return m_weaponRoot; } set { m_weaponRoot = value; } }
    GameObject m_projectilePrefab;
    public GameObject ProjectilePrefab { get { return m_projectilePrefab; } set { m_projectilePrefab = value; } }
    Quaternion m_bulletRotation;
    public Quaternion RotationAngle { get { return m_bulletRotation; } set { m_bulletRotation = value; } }
    string m_bulletTag;
    public string BulletTag { get { return m_bulletTag; } set { m_bulletTag = value; } }
    int m_bulletCount;
    public int BulletCount { get { return m_bulletCount; } set { m_bulletCount = value; } }
    private float m_shotCooldown = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetWeaponData(WeaponData data)
    {
        GameObject actualMesh = m_weaponRoot.transform.Find("WeaponMesh").gameObject;
        GameObject actualBarrel = m_weaponRoot.transform.Find("ProjectileSpawner").gameObject;

        GameObject oldWeapon = actualMesh.transform.GetChild(0).gameObject;
        if (oldWeapon != null)
        {
            GameObject.Destroy(oldWeapon);
        }

        GameObject newMesh = Instantiate(data.weapon);
        newMesh.transform.parent = actualMesh.transform;
        newMesh.transform.position = Vector3.zero;
        newMesh.transform.localPosition = Vector3.zero;
        newMesh.transform.localRotation = Quaternion.identity;

        m_weaponRoot.transform.localPosition = data.gripPosition;
        m_weaponRoot.transform.localRotation = Quaternion.Euler(data.gripRotation);
        actualBarrel.transform.localPosition = data.barrelPosition;

        m_shotCooldown = data.rateOfFire;
    }
    
    public void Attack()
    {
        ProjectileFactory.Instance.SpawnProjectile(ProjectilePrefab, WeaponRoot.transform.Find("ProjectileSpawner").transform.position, 3000, WeaponRoot.transform.rotation, BulletTag);
    }
}
