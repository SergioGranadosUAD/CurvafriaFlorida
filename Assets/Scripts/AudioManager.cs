using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //Clase sin uso de momento, más que para mantener separados los audios en caso de implementación propia más adelante.
    private static AudioManager m_instance;

    public static AudioManager Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType(typeof(AudioManager)) as AudioManager;
            }
            return m_instance;
        }
    }

    [SerializeField] private List<AudioClip> deathSounds = new List<AudioClip>();
    public List<AudioClip> DeathSounds { get { return deathSounds; } }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public AudioClip GetRandomDeathSound()
    {
        int rand = Random.Range(0, DeathSounds.Count);
        return DeathSounds[rand];
    }
}
