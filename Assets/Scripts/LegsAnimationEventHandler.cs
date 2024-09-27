using UnityEngine;

public class LegsAnimationEventHandler : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] walkingSounds;
    public AudioClip[] runningSounds;

    public void PlayWalkSound()
    {
        PlayRandomSound(walkingSounds);
    }

    public void PlayRunSound()
    {
        PlayRandomSound(runningSounds);
    }

    private void PlayRandomSound(AudioClip[] clips)
    {
        if (clips.Length > 0)
            audioSource.PlayOneShot(clips[Random.Range(0, clips.Length)]);
    }
}
