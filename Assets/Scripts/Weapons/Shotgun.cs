using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : MonoBehaviour, IWeapon
{
    private GameObject m_weaponRoot;
    private GameObject m_spawnerLocation;

    private ParticleSystem m_muzzleFlash;
    private Light m_muzzleLight;
    private float m_muzzleLightInterval = 0;
    private AudioSource m_shotSound;
    private AudioClip m_shotSoundClip;

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

    void Update()
    {
        m_muzzleLightInterval += Time.deltaTime;
        if (m_muzzleLightInterval >= 0.1f && m_muzzleLight != null)
        {
            m_muzzleLight.enabled = false;
        }
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

        m_muzzleFlash = m_spawnerLocation.transform.Find("MuzzleFlashEmmiter").GetComponent<ParticleSystem>();
        m_muzzleLight = m_spawnerLocation.transform.Find("MuzzleFlashLight").GetComponent<Light>();
        m_shotSound = m_spawnerLocation.transform.Find("WeaponSound").GetComponent<AudioSource>();

        m_shotSoundClip = data.shotSound;
    }

    public bool Attack()
    {
        if (m_canShoot && (m_bulletCount > 0 || BottomlessClip))
        {
            m_muzzleFlash.Play();
            m_muzzleLight.enabled = true;
            m_muzzleLightInterval = 0;
            m_shotSound.PlayOneShot(m_shotSoundClip);

            float angleX = WeaponRoot.transform.eulerAngles.x;
            float angleY = WeaponRoot.transform.eulerAngles.y;
            float angleZ = WeaponRoot.transform.eulerAngles.z;

            for(int i = 0; i < 5; ++i)
            {
                Quaternion spreadValue = Quaternion.Euler(Random.Range(angleX - m_spreadAngle, angleX + m_spreadAngle),
                                                          Random.Range(angleY - m_spreadAngle, angleY + m_spreadAngle),
                                                          Random.Range(angleZ - m_spreadAngle, angleZ + m_spreadAngle));
                ProjectileFactory.Instance.SpawnProjectile(m_spawnerLocation.transform.position, 3000, spreadValue, BulletTag);
            }
            if(!BottomlessClip)
            {
                m_bulletCount--;
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
