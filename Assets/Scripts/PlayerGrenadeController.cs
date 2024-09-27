using UnityEngine;

public class PlayerGrenadeController : MonoBehaviour
{
    public GameObject grenadeThrowPoint;
    public GameObject grenadePrefab;
    public float maxThrowDistance = 5f;
    public float throwVelocity = 10f;
    public Animator animator;
    public Camera cam;

    public int maxGrenadeCount = 3;

    public AudioClip[] throwSounds;
    private AudioSource _as;

    private PlayerManager _pm;
    private InputManager _im;

    private void Start()
    {
        _as = GetComponent<AudioSource>();

        _pm = PlayerManager.instance;
        _im = InputManager.instance;

        _pm.SetGrenades(maxGrenadeCount);
    }

    private void Update()
    {
        if (_im.GetMouseButtonDown(1) && !_pm.busy && _pm.Grenades > 0)
        {
            _pm.SetGrenades(_pm.Grenades - 1);
            animator.SetTrigger("Grenade");
        }
    }

    private void OnDrawGizmos()
    {
        if (grenadeThrowPoint == null) return;
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(grenadeThrowPoint.transform.position, maxThrowDistance);
    }

    public void ThrowGrenade()
    {
        GameObject grenade = Instantiate(grenadePrefab);
        grenade.transform.position = grenadeThrowPoint.transform.position;
        grenade.transform.rotation = transform.rotation;
        grenade.SetActive(true);

        grenade.GetComponent<Rigidbody2D>().velocity = transform.up * throwVelocity;

        Vector3 mousePosition = cam.ScreenToWorldPoint(_im.MousePosition());
        mousePosition.z = 0;
        Vector3 difference = mousePosition - grenadeThrowPoint.transform.position;
        float throwDistance = Mathf.Min(maxThrowDistance, difference.magnitude);
        grenade.GetComponent<GrenadeController>().SetMaxDistance(throwDistance);

        PlayRandomSound(throwSounds);
    }

    public void AddGrenades(int grenades)
    {
        _pm.SetGrenades(Mathf.Min(_pm.Grenades + grenades, maxGrenadeCount));
    }

    public bool IsFullGrenades()
    {
        return _pm.Grenades == maxGrenadeCount;
    }

    private void PlayRandomSound(AudioClip[] sounds)
    {
        _as.PlayOneShot(sounds[Random.Range(0, sounds.Length)]);
    }
}
