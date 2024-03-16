using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour, IWeapon
{
    GameObject m_weaponRoot;
    GameObject m_spawnerLocation;
    public GameObject WeaponRoot { get { return m_weaponRoot; } set { m_weaponRoot = value; } }
    Quaternion m_bulletRotation;
    public Quaternion RotationAngle { get { return m_bulletRotation; } set { m_bulletRotation = value; } }
    string m_bulletTag;
    public string BulletTag { get { return m_bulletTag; } set { m_bulletTag = value; } }
    int m_bulletCount;
    public int BulletCount { get { return m_bulletCount; } set { m_bulletCount = value; } }
    private bool m_bottomlessClip;
    public bool BottomlessClip { get { return m_bottomlessClip; } set { m_bottomlessClip = value; } }
    private float m_shotCooldown = 0;
    private bool m_canShoot = true;
    private float m_spreadAngle = 0;

    //bool m_Started;
    //public LayerMask m_LayerMask;
    //
    //void Start()
    //{
    //    //Use this to ensure that the Gizmos are being drawn when in Play Mode.
    //    m_Started = true;
    //}
    //
    //void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    //Check that it is being run in Play Mode, so it doesn't try to draw this in Editor mode
    //    if (m_Started)
    //        //Draw a cube where the OverlapBox is (positioned where your GameObject is as well as a size)
    //        Gizmos.DrawWireCube(m_spawnerLocation.transform.position, Vector3.one);
    //}

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

    public bool Attack()
    {
        if (m_canShoot)
        {
            Collider[] hitColliders = Physics.OverlapBox(m_spawnerLocation.transform.position, Vector3.one, WeaponRoot.transform.rotation);
            for(int i = 0;  i < hitColliders.Length; i++)
            {
                if(BulletTag.Equals("Allied") && hitColliders[i].CompareTag("Enemy"))
                {
                    Enemy enemyRef = hitColliders[i].GetComponent<Enemy>();
                    if (enemyRef != null)
                    {
                        enemyRef.DamageEnemy();
                    }
                }
                else if(BulletTag.Equals("Hostile") && hitColliders[i].CompareTag("Player"))
                {
                    Player playerRef = hitColliders[i].GetComponent<Player>();
                    if(playerRef != null)
                    {
                        playerRef.DamagePlayer();
                    }
                }
            }
            StartCoroutine(WaitForCooldown());
            return true;
        }
        return false;
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
