using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager m_instance;

    public static GameManager Instance
    {
        get
        {
            if(m_instance == null)
            {
                m_instance = FindObjectOfType(typeof(GameManager)) as GameManager;
            }
            return m_instance;
        }
    }
    
    private GameObject m_player;
    // Start is called before the first frame update
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {

        if(m_player == null)
        {
            m_player = GameObject.Find("Player");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject GetPlayer()
    {
        return m_player;
    }
}
