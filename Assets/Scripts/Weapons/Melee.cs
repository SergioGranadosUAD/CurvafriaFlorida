using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour, IWeapon
{
    GameObject m_weaponRoot;
    GameObject m_spawnerLocation;
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
    private bool m_canShoot = true;
    private float m_spreadAngle = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetWeaponData(WeaponData data, int currentAmmo)
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

        m_spreadAngle = data.spreadAngle;
        m_spawnerLocation = WeaponRoot.transform.Find("ProjectileSpawner").gameObject;
        m_shotCooldown = 1 / data.rateOfFire;
        m_bulletCount = currentAmmo;
    }

    public void Attack()
    {
        if (m_canShoot)
        {
            //ProjectileFactory.Instance.SpawnProjectile(ProjectilePrefab, m_spawnerLocation.transform.position, 3000, spreadValue, BulletTag);
            StartCoroutine(WaitForCooldown());
        }

    }

    private IEnumerator WaitForCooldown()
    {
        m_canShoot = false;
        float currentCooldown = 0;
        while (currentCooldown <= m_shotCooldown)
        {
            currentCooldown += Time.deltaTime;
            yield return null;
        }
        m_canShoot = true;
    }
}
