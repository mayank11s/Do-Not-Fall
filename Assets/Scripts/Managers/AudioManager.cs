using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioSource sfxAudioSource;
    [SerializeField] private AudioClip clickSfx;
    [SerializeField] private AudioClip jumpSfx;
    [SerializeField] private AudioClip popSfx;
    [SerializeField] private AudioClip victorySfx;
    [SerializeField] private AudioClip losingSfx;

    void Awake()
    {
        Instance = this;
    }
    public void PlayClickSfx()
    {
        sfxAudioSource.PlayOneShot(clickSfx);
    }
    public void PlayJumpSfx()
    {
        sfxAudioSource.PlayOneShot(jumpSfx);
    }
    public void PlayPopSfx()
    {
        sfxAudioSource.PlayOneShot(popSfx);
    }
    public void PlayVictorySfx()
    {
        sfxAudioSource.PlayOneShot(victorySfx);
    }
    public void PlayLosingSfx()
    {
        sfxAudioSource.PlayOneShot(losingSfx);
    }
}
