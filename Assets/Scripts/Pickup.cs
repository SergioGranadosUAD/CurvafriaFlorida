using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public delegate void OnPickupDestroyed(GameObject pickup);
    public event OnPickupDestroyed onPickupDestroyed;

    [SerializeField] private WeaponData m_weaponData;
    public WeaponData WeaponData
    {
        get { return m_weaponData; }
    }

    private GameObject m_weaponObject;
    private GameObject m_weaponOutline;
    private int m_currentAmmo;
    public int CurrentAmmo { get { return m_currentAmmo; } }
    // Start is called before the first frame update
    void Start()
    {
        if(m_weaponObject == null)
        {
            SetPickupType(m_weaponData);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.transform.CompareTag("Player"))
        {
            Player playerRef = collision.transform.GetComponent<Player>();
            playerRef.PickupsNearby.Add(gameObject);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            Player playerRef = collision.transform.GetComponent<Player>();
            GameObject foundObject;
            //Lambda que encuentra el objeto equivalente a la colisión.
            foundObject = playerRef.PickupsNearby.Find(obj => obj == collision.gameObject);
            if (foundObject != null)
            {
                playerRef.PickupsNearby.Remove(foundObject);
            }
        }
    }

    public void SetPickupType(WeaponData data)
    {
        m_weaponData = data;
        m_currentAmmo = m_weaponData.maxAmmo;

        m_weaponObject = GameObject.Instantiate(data.weapon);
        m_weaponObject.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
        m_weaponObject.transform.parent = transform;
        m_weaponObject.transform.localPosition = Vector3.zero;

        //m_weaponOutline = GameObject.Instantiate(weapon);
        //m_weaponObject.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
        //m_weaponOutline.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
    }

    private void OnDestroy()
    {
        onPickupDestroyed.Invoke(gameObject);
    }
}
