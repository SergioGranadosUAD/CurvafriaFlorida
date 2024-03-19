using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private static UIManager m_instance;

    public static UIManager Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType(typeof(UIManager)) as UIManager;
            }
            return m_instance;
        }
    }

    [SerializeField] private GameObject m_hud;
    [SerializeField] private GameObject m_pauseMenu;
    [SerializeField] private GameObject m_deathScreen;
    [SerializeField] private List<Sprite> m_weaponSprites;

    private Image m_weaponImage;
    private TMP_Text m_ammoText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateWeaponSprite(string type)
    {
        switch(type)
        {
            case "Melee":
                m_weaponImage.sprite = m_weaponSprites[0];
                break;
            case "Pistol":
                m_weaponImage.sprite = m_weaponSprites[1];
                break;
            case "Rifle":
                m_weaponImage.sprite = m_weaponSprites[2];
                break;
            case "Shotgun":
                m_weaponImage.sprite = m_weaponSprites[3];
                break;
            default:
                m_weaponImage.sprite = m_weaponSprites[0];
                break;
        }
    }

    public void UpdateAmmoText(int currentAmmo, int maxAmmo)
    {
        m_ammoText.text = currentAmmo.ToString() + "/" + maxAmmo.ToString();
    }

    public void ShowPauseMenu(bool active)
    {
        m_pauseMenu.SetActive(active);
    }

    public void ShowGameOverMenu(bool active)
    {
        m_deathScreen.SetActive(active);
    }

    public void ResumeGame()
    {
        GameManager.Instance.Paused = false;
    }

    public void QuitGame()
    {
        SceneManager.LoadScene(0);
    }

    public void UnbindUIManagerFromPlayer()
    {
        GameManager.Instance.Player.OnWeaponSwitched -= UpdateWeaponSprite;
        GameManager.Instance.Player.OnWeaponShot -= UpdateAmmoText;
    }
    public void SetupUIManager()
    {
        m_weaponImage = m_hud.transform.Find("WeaponIcon").GetComponent<Image>();
        m_ammoText = m_hud.transform.Find("AmmoCount").GetComponent<TMP_Text>();

        GameManager.Instance.Player.OnWeaponSwitched += UpdateWeaponSprite;
        GameManager.Instance.Player.OnWeaponShot += UpdateAmmoText;
    }
}
