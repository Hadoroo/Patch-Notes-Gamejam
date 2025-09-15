using UnityEngine;
using System.Collections.Generic; // You'll need this for the Dictionary

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    // List Audio Effect
    public List<AudioClip> soundEffects;
    // Main Audio Source
    public AudioSource sfxSource;

    // Music
    public AudioClip backgroundMusic;
    private AudioSource musicSource;

    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
            musicSource = gameObject.AddComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }

    }

    public void PlayBackgroundMusic()
    {
        
        if (musicSource != null && backgroundMusic != null)
        {
            musicSource.clip = backgroundMusic;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    public void StopBackgroundMusic()
    {
        if (musicSource != null)
        {
            musicSource.Stop();
        }
    }



    // Summary : Audio Clip stored on list, play effect by index
    public void PlaySoundEffect(int index)
    {
        if (sfxSource != null && index >= 0 && index < soundEffects.Count)
        {
            sfxSource.PlayOneShot(soundEffects[index]);
        }
    }
    public void PlayRandomBulletSound()
    {
        if (sfxSource != null && soundEffects.Count > 0)
        {
            // Hardcode randomizer, 1-5 are bullet sounds
            int randomIndex = Random.Range(1, 5);
            sfxSource.PlayOneShot(soundEffects[randomIndex]);
        }
    }
    
}