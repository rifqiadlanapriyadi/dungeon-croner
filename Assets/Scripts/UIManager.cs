using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public TMP_Text healthText;
    public TMP_Text pistolClipText;
    public TMP_Text rifleClipText;
    public TMP_Text ammoText;
    public TMP_Text grenadeText;
    public TMP_Text doorsOpenNotificationText;

    public GameObject youWinUI;
    public GameObject gameOverUI;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void Start()
    {
        gameOverUI.SetActive(false);
    }

    public void SetHealthText(int health)
    {
        healthText.text = health.ToString();
    }

    public void SetPistolClipText(int pistolClip)
    {
        pistolClipText.text = pistolClip.ToString();
    }

    public void SetRifleClipText(int rifleClip)
    {
        rifleClipText.text = rifleClip.ToString();
    }

    public void SetAmmoText(int ammo)
    {
        ammoText.text = ammo.ToString();
    }

    public void SetGrenadeText(int grenade)
    {
        grenadeText.text = grenade.ToString();
    }

    public void SetDoorsOpenNotificationActive(bool active)
    {
        doorsOpenNotificationText.gameObject.SetActive(active);
    }
    public void ShowYouWinUI(bool show)
    {
        youWinUI.SetActive(show);
    }

    public void ShowGameOverUI(bool show)
    {
        gameOverUI.SetActive(show);
    }
}
