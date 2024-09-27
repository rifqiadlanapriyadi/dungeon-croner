using UnityEngine;

public class PlayerWeaponSwapController : MonoBehaviour
{
    public Animator animator;

    private PlayerManager _pm;
    private InputManager _im;

    public AudioClip pistolSound;
    public AudioClip rifleSound;
    private AudioSource _as;

    private void Start()
    {
        _pm = PlayerManager.instance;
        _im = InputManager.instance;

        _as = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (_im.GetKeyDown(KeyCode.Q) && !_pm.busy)
        {
            animator.SetTrigger("Switch");
            animator.SetBool("Rifle", !_pm.rifle);
        }
    }

    public void ToggleWeapon()
    {
        _pm.rifle = !_pm.rifle;
        _as.PlayOneShot(_pm.rifle ? rifleSound : pistolSound);
    }
}
