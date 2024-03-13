using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public delegate void OnProjectileDestroyed(GameObject projectile);
    public event OnProjectileDestroyed onProjectileDestroyed;
   

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

    private void OnTriggerEnter(Collider collision)
    {
        if(transform.CompareTag("Allied") && collision.transform.CompareTag("Enemy"))
        {
            Enemy enemyRef = collision.transform.GetComponent<Enemy>();
            enemyRef.DamageEnemy();
            GameObject.Destroy(gameObject);
        }
        else if (transform.CompareTag("Hostile") && collision.transform.CompareTag("Player"))
        {
            Player playerRef = collision.transform.GetComponent<Player>();
            playerRef.DamagePlayer();
            GameObject.Destroy(gameObject);
        }
        else if(collision.transform.CompareTag("Scenario"))
        {
            GameObject.Destroy(gameObject);
        }
    }

    public void SetProjectileData(float speed, Quaternion rotation, string tag)
    {
        m_speed = speed;
        transform.rotation = rotation;
        gameObject.tag = tag;
    }

    private void OnDestroy()
    {
        onProjectileDestroyed.Invoke(gameObject);
    }
}
