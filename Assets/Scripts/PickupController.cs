using UnityEngine;
using System.Collections;

public class PickupController : MonoBehaviour
{
    public bool respawn = false;
    public float respawnTimer = 30f;

    public AudioClip[] pickupSounds;
    private AudioSource _as;

    public Collider2D pickupCollider;
    public SpriteRenderer sprite;

    protected virtual void Start()
    {
        _as = GetComponent<AudioSource>();
    }

    protected void Deactivate()
    {
        AudioClip clip = PlayRandomSound(pickupSounds);
        if (!respawn)
        {
            pickupCollider.enabled = false;
            sprite.enabled = false;
            StartCoroutine(DestroyAfter(clip.length));
        }
        else
        {
            Invoke("Respawn", respawnTimer);
            SetComponentsActive(false);
        }
    }

    private IEnumerator DestroyAfter(float clipLength)
    {
        yield return new WaitForSeconds(clipLength);
        Destroy(gameObject);
    }

    private void Respawn()
    {
        SetComponentsActive(true);
    }

    private void SetComponentsActive(bool active)
    {
        pickupCollider.enabled = active;
        sprite.enabled = active;
        enabled = active;
    }

    private AudioClip PlayRandomSound(AudioClip[] sounds)
    {
        AudioClip clip = sounds[Random.Range(0, sounds.Length)];
        _as.PlayOneShot(clip);
        return clip;
    }
}
