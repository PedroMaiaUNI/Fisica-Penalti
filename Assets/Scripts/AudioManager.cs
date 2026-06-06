using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Music")]
    public AudioSource musicSource;

    [Header("SFX")]
    public AudioSource sfxSource;

    [Header("Sounds")]
    public AudioClip kickSound;
    public AudioClip postSound;
    public AudioClip goalSound;
    public AudioClip saveSound;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void PlayKick()
    {
        sfxSource.PlayOneShot(kickSound);
    }

    public void PlayPost()
    {
        sfxSource.PlayOneShot(postSound);
    }

    public void PlayGoal()
    {
        sfxSource.PlayOneShot(goalSound);
    }

    public void PlaySave()
    {
        sfxSource.PlayOneShot(saveSound);
    }
}
