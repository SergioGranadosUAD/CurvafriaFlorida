using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental.FileFormat;
using UnityEngine;

enum PickupType
{
    PISTOL,
    RIFLE,
    SHOTGUN
}
public class PickupSpawner : MonoBehaviour, ISpawner
{
    [SerializeField] private PickupType pickupType = PickupType.PISTOL;
    private WeaponData m_pickupData;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnEntity()
    {
        switch (pickupType)
        {
            case PickupType.PISTOL:
                m_pickupData = Resources.Load<WeaponData>("ScriptableObjects/Weapons/Pistol");
                break;
            case PickupType.RIFLE:
                m_pickupData = Resources.Load<WeaponData>("ScriptableObjects/Weapons/Rifle");
                break;
            case PickupType.SHOTGUN:
                m_pickupData = Resources.Load<WeaponData>("ScriptableObjects/Weapons/Shotgun");
                break;
            default:
                m_pickupData = Resources.Load<WeaponData>("ScriptableObjects/Weapons/Melee");
                break;
        }
        PickupFactory.Instance.SpawnPickup(transform.position, m_pickupData, m_pickupData.maxAmmo);
    }
}
