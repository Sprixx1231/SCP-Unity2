using System;
using UnityEngine;
using Unity.Audio;


public class SoundManager : MonoBehaviour
{
    [SerializeField] private Sound[] _sound;

    public static SoundManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        
        DontDestroyOnLoad(gameObject);
        
        foreach (Sound s in _sound)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            
            s.source.clip = s.audioClip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(_sound, _sound => _sound.name == name);

        if (s == null)
        {
            Debug.LogWarning("Sound " + name + " was not found");
        }

        
        s.source.Play();
    }
}