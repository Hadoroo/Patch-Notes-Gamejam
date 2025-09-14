using UnityEngine;
using System.Collections.Generic; // You'll need this for the Dictionary

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    // List Audio Effect
    public List<AudioClip> soundEffects;
    // Main Audio Source
     public AudioSource sfxSource;

    private void Awake()
    {
    
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
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
}