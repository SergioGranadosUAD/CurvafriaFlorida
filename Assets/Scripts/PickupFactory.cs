using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupFactory : MonoBehaviour
{
    private static PickupFactory m_instance;
    private GameObject m_pickupAsset;
    public GameObject PickupAsset
    {
        get
        {
            if(m_pickupAsset == null)
            {
                m_pickupAsset = Resources.Load("Prefabs/WeaponPickup") as GameObject;
            }
            return m_pickupAsset;
        }
    }

    public static PickupFactory Instance
    {
        get
        {
            if (m_instance == null)
            {
                GameObject gameObject = new GameObject();
                gameObject.name = "Pickup Factory";
                m_instance = gameObject.AddComponent<PickupFactory>();
            }
            return m_instance;
        }
    }

    public List<GameObject> m_pickupList;
    void Awake()
    {
        if (m_pickupList == null)
        {
            m_pickupList = new List<GameObject>();
        }
    }

    public GameObject SpawnPickup(Vector3 position, WeaponData data, int bulletsLeft)
    {
        GameObject newPickup = Instantiate(PickupAsset, position, Quaternion.identity);
        Pickup pickupRef = newPickup.GetComponent<Pickup>();
        pickupRef.SetPickupType(data);
        m_pickupList.Add(newPickup);
        
        pickupRef.onPickupDestroyed += RemovePickupFromList;
        
        return newPickup;
    }

    private void RemovePickupFromList(GameObject pickup)
    {
        m_pickupList.Remove(pickup);
    }

    public void ClearPickupList()
    {
        if (m_pickupList.Count > 0)
        {
            foreach (GameObject pickup in m_pickupList)
            {
                Destroy(pickup);
            }
            m_pickupList.Clear();
        }
    }
}
