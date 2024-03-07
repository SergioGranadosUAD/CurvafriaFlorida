using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float m_speed = 2f;
    private Rigidbody m_rigidBody;
    private MeshRenderer m_renderer;

    private float despawnTimer = 0;

    private float timeToDespawn = 3;

    // Start is called before the first frame update
    void Start()
    {
        m_rigidBody = GetComponent<Rigidbody>();
        m_renderer = GetComponent<MeshRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        m_rigidBody.velocity = transform.forward * m_speed * Time.deltaTime;

        if(!m_renderer.isVisible)
        {
            despawnTimer += Time.deltaTime;
            if (despawnTimer >= timeToDespawn)
            {
                GameObject.Destroy(gameObject);
            }
        }

        
    }

    private void OnCollisionEnter(Collision collision)
    {
        
    }

    public void SetProjectileData(float speed, Quaternion rotation, string tag)
    {
        m_speed = speed;
        transform.rotation = rotation;
        gameObject.tag = tag;
    }
}
