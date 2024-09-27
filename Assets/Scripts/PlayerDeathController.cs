using UnityEngine;

public class PlayerDeathController : MonoBehaviour
{
    public Animator torsoAnimator;
    public Animator legsAnimator;

    public AudioClip[] deathSounds;
    private AudioSource _as;

    private void Start()
    {
        _as = GetComponent<AudioSource>();
    }

    public void HandleDeath()
    {
        if (deathSounds.Length > 0)
            _as.PlayOneShot(deathSounds[Random.Range(0, deathSounds.Length)]);

        torsoAnimator.SetTrigger("Die");
        torsoAnimator.GetComponent<SpriteRenderer>().sortingLayerName = "Corpse";

        Destroy(legsAnimator.gameObject);

        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().simulated = false;
        GetComponent<PlayerController>().enabled = false;
        GetComponent<PlayerRunController>().enabled = false;
        GetComponent<PlayerMeleeController>().enabled = false;
        GetComponent<PlayerShootController>().enabled = false;
        GetComponent<PlayerGrenadeController>().enabled = false;
        GetComponent<PlayerWeaponSwapController>().enabled = false;
        GetComponent<PlayerReloadController>().enabled = false;
        GetComponent<PlayerHurtController>().enabled = false;
    }

    public void OnDeathAnimationFinish()
    {
        torsoAnimator.enabled = false;
        torsoAnimator.gameObject.GetComponent<PlayerAnimationEventHandler>().enabled = false;
    }
}
