using System.Linq;
using UnityEngine;

public class PlayerShootController : MonoBehaviour
{
    public Animator animator;

    public float shootPeriod = .5f;
    private float _nextPistolShootTime;
    private float _nextRifleShootTime;

    public GameObject pistolShootPoint;
    public int pistolClipSize = 10;

    public GameObject rifleShootPoint;
    public float spreadAngle = 10f;
    public float rifleMaxBulletDistance = 7.5f;
    public int rifleBulletsPerShot = 4;
    public int rifleClipSize = 4;

    public int maxAmmo = 30;

    public AudioClip[] pistolShootSounds;
    public AudioClip[] rifleShootSounds;
    public AudioClip[] pistolReloadSounds;
    public AudioClip[] rifleReloadSounds;
    private AudioSource _as;

    private InputManager _im;
    private BulletPool _bp;
    private PlayerManager _pm;

    private void Start()
    {
        _im = InputManager.instance;
        _bp = BulletPool.instance;
        _pm = PlayerManager.instance;

        _as = GetComponent<AudioSource>();

        _nextPistolShootTime = Time.time;
        _nextRifleShootTime = Time.time;

        _pm.SetPistolClip(pistolClipSize);
        _pm.SetRifleClip(rifleClipSize);
        _pm.SetAmmo(maxAmmo);
    }

    private void Update()
    {
        if (_im.GetMouseButtonDown(0) && !_pm.busy)
        {
            if (!_pm.rifle && Time.time >= _nextPistolShootTime && _pm.PistolClip > 0)
            {
                _nextPistolShootTime = Time.time + shootPeriod;
                animator.SetTrigger("Shoot");
            } else if (_pm.rifle && Time.time >= _nextRifleShootTime && _pm.RifleClip > 0)
            {
                _nextRifleShootTime = Time.time + shootPeriod;
                animator.SetTrigger("Shoot");
            }
        }
    }

    public void Shoot()
    {
        if (!_pm.rifle)
        {
            _bp.DequeueFromPool(pistolShootPoint.transform.position, transform.rotation, 0);
            _pm.SetPistolClip(_pm.PistolClip - 1);
            PlayRandomSound(pistolShootSounds);
        }
        else
        {
            for (int i = 0; i < rifleBulletsPerShot; i++)
            {
                float spread = Random.Range(-spreadAngle, spreadAngle);
                Quaternion spreadRotation = Quaternion.AngleAxis(spread, Vector3.forward);
                _bp.DequeueFromPool(pistolShootPoint.transform.position, transform.rotation * spreadRotation, rifleMaxBulletDistance);
            }
            _pm.SetRifleClip(_pm.RifleClip - 1);
            PlayRandomSound(rifleShootSounds);
        }
    }

    public void ReloadPistol()
    {
        int added = Mathf.Min(pistolClipSize - _pm.PistolClip, _pm.Ammo);
        _pm.SetPistolClip(_pm.PistolClip + added);
        _pm.SetAmmo(_pm.Ammo - added);
        PlayRandomSound(pistolReloadSounds);
    }

    public void ReloadRifle()
    {
        int added = Mathf.Min(rifleClipSize - _pm.RifleClip, _pm.Ammo / rifleBulletsPerShot);
        _pm.SetRifleClip(_pm.RifleClip + added);
        _pm.SetAmmo(_pm.Ammo - added * rifleBulletsPerShot);
        PlayRandomSound(rifleReloadSounds);
    }

    public void AddAmmo(int addedAmmo)
    {
        _pm.SetAmmo(Mathf.Min(_pm.Ammo + addedAmmo, maxAmmo));
    }

    public bool IsFullAmmo()
    {
        return _pm.Ammo == maxAmmo;
    }

    private void OnDrawGizmos()
    {
        if (rifleShootPoint == null) return;
        Gizmos.color = Color.blue;
        Vector3 forwardDirection = rifleShootPoint.transform.up;
        Vector3 spreadMin = Quaternion.Euler(0, 0, -spreadAngle) * forwardDirection;
        Vector3 spreadMax = Quaternion.Euler(0, 0, spreadAngle) * forwardDirection;
        Gizmos.DrawRay(rifleShootPoint.transform.position, forwardDirection * rifleMaxBulletDistance);
        Gizmos.DrawRay(rifleShootPoint.transform.position, spreadMin * rifleMaxBulletDistance);
        Gizmos.DrawRay(rifleShootPoint.transform.position, spreadMax * rifleMaxBulletDistance);
    }

    private void PlayRandomSound(AudioClip[] sounds)
    {
        _as.PlayOneShot(sounds[Random.Range(0, sounds.Length)]);
    }
}
