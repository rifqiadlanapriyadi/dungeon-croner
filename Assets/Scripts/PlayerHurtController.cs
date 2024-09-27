using UnityEngine;

public class PlayerHurtController : MonoBehaviour
{
    public Animator torsoAnimator;

    public AudioClip[] hurtSounds;
    private AudioSource _as;

    private void Start()
    {
        _as = GetComponent<AudioSource>();
    }

    public void OnHit()
    {
        _as.PlayOneShot(hurtSounds[Random.Range(0, hurtSounds.Length)]);
        torsoAnimator.SetTrigger("Hurt");
    }
}
