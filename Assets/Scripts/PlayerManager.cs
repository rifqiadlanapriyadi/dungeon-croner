using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    public GameObject player;

    public int maxHealth = 3;

    private int _health;
    public int Health
    {
        get { return _health; }
        private set { _health = value; }
    }
    private int _pistolClip;
    public int PistolClip
    {
        get { return _pistolClip; }
        private set { _pistolClip = value;}
    }
    private int _rifleClip;
    public int RifleClip
    {
        get { return _rifleClip; }
        private set { _rifleClip = value; }
    }
    private int _ammo;
    public int Ammo
    {
        get { return _ammo; }
        private set { _ammo = value; }
    }
    private int _grenades;
    public int Grenades
    {
        get { return _grenades; }
        private set { _grenades = value; }
    }

    private bool _isInvincible;
    public bool IsInvincible
    {
        get { return _isInvincible; }
        set { _isInvincible = value; }
    }

    private PlayerInvincibilityController _pic;
    private PlayerHurtController _phc;
    private PlayerDeathController _pdc;

    [HideInInspector]
    public bool busy;
    [HideInInspector]
    public bool rifle;
    [HideInInspector]
    public bool reloading;
    [HideInInspector]
    public bool running;

    [HideInInspector]
    public bool hasUsedDoor;
    [HideInInspector]
    public LevelDoorDirection lastEnteredDoorDir;

    private GameManager _gm;
    private UIManager _ui;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void Start()
    {
        _ui = UIManager.instance;
        _gm = GameManager.instance;

        SetHealth(maxHealth);

        busy = false;
        rifle = false;
        reloading = false;
        running = false;

        hasUsedDoor = false;
        lastEnteredDoorDir = LevelDoorDirection.North;

        _isInvincible = false;

        _phc = player.GetComponent<PlayerHurtController>();
        _pdc = player.GetComponent<PlayerDeathController>();
        _pic = player.GetComponent<PlayerInvincibilityController>();
    }

    public bool IsAlive()
    {
        return Health > 0;
    }

    public bool IsFullHealth()
    {
        return Health == maxHealth;
    }

    public void AddHealth(int addedHealth)
    {
        SetHealth(Mathf.Min(Health + addedHealth, maxHealth));
    }

    public void HandleHit()
    {
        if (IsAlive() && !IsInvincible)
        {
            SetHealth(Health - 1);
            if (IsAlive())
            {
                _phc.OnHit();
                _pic.StartInvincibility();
            }
            else
            {
                _pdc.HandleDeath();
                _gm.GameOver();
            }
        }
    }

    public void SetHealth(int health)
    {
        Health = health;
        _ui.SetHealthText(Health);
    }

    public void SetPistolClip(int pistolClip)
    {
        PistolClip = pistolClip;
        _ui.SetPistolClipText(PistolClip);
    }

    public void SetRifleClip(int rifleClip)
    {
        RifleClip = rifleClip;
        _ui.SetRifleClipText(RifleClip);
    }

    public void SetAmmo(int ammo)
    {
        Ammo = ammo;
        _ui.SetAmmoText(Ammo);
    }

    public void SetGrenades(int grenades)
    {
        Grenades = grenades;
        _ui.SetGrenadeText(Grenades);
    }
}
