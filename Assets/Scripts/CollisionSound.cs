using UnityEngine;

public class CollisionSound : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioClip collisionClip;
    [Range(0f, 1f)] public float volume = 1f;

    private void OnCollisionEnter(Collision collision)
    {
        PlaySound();
    }


    private void PlaySound()
    {
        if (audioSource != null && collisionClip != null)
        {
            audioSource.PlayOneShot(collisionClip, volume);
        }
    }
}
