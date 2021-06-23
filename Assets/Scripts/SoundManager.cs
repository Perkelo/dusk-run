using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public enum AudioFX
    {
        Five,
        Four,
        Three,
        Two,
        One,
        Brawl
    }

    public static SoundManager instance;
    private const short sourcesCount = 10;
    private AudioSource[] sources;
    [SerializeField] private AudioClip five;
    [SerializeField] private AudioClip four;
    [SerializeField] private AudioClip three;
    [SerializeField] private AudioClip two;
    [SerializeField] private AudioClip one;
    [SerializeField] private AudioClip brawl;
    private short counter = 0;

    void Start() {
        instance = this;

        sources = new AudioSource[sourcesCount];
        for (short i = 0; i < sourcesCount; i++)
        {
            sources[i] = gameObject.AddComponent<AudioSource>();
        }
    }

    public void PlaySoundFX(AudioFX clip)
    {
        AudioClip fx;
        switch (clip)
        {
            case AudioFX.Five:
                fx = five;
                break;
            case AudioFX.Four:
                fx = four;
                break;
            case AudioFX.Three:
                fx = three;
                break;
            case AudioFX.Two:
                fx = two;
                break;
            case AudioFX.One:
                fx = one;
                break;
            case AudioFX.Brawl:
                fx = brawl;
                break;
            default:
                fx = brawl;
                break;
        }

        sources[counter].Stop();
        sources[counter].clip = fx;
        sources[counter].Play();
        counter = (short)((counter + 1) % sourcesCount);
    }
}
