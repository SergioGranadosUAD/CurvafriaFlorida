using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    private GameObject m_playerPrefab;
    // Start is called before the first frame update
    void Start()
    {
        m_playerPrefab = Resources.Load<GameObject>("Prefabs/Player");
        GameObject player = GameObject.Instantiate(m_playerPrefab, transform.position, transform.rotation);
        player.name = "Player";
        GameManager.Instance.Camera.SetCameraTarget(player.transform);
        GameObject.Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
