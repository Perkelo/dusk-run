using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public enum AudioFX
    {
        Intro,
        Five,
        Four,
        Three,
        Two,
        One,
        Brawl,
        Mouseover
    }

    public enum Music
    {
        Level1
    }

    [HideInInspector] public static SoundManager instance;
    private const short sourcesCount = 10;
    private AudioSource[] fxSources;
    private AudioSource musicSource;
    public float fxVolume = 1.0f;
    public float musicVolume = 1.0f;
    [Header("Effects")]
    [SerializeField] private AudioClip intro;
    [SerializeField] private AudioClip five;
    [SerializeField] private AudioClip four;
    [SerializeField] private AudioClip three;
    [SerializeField] private AudioClip two;
    [SerializeField] private AudioClip one;
    [SerializeField] private AudioClip brawl;
    [SerializeField] private AudioClip mouseover;
    [Header("Music")]
    [SerializeField] private AudioClip level1Intro;
    [SerializeField] private AudioClip level1Loop;
    private short counter = 0;

    private void Awake()
    {
        instance = this;

        fxSources = new AudioSource[sourcesCount];
        for (short i = 0; i < sourcesCount; i++)
        {
            fxSources[i] = gameObject.AddComponent<AudioSource>();
        }
        musicSource = gameObject.AddComponent<AudioSource>();
    }

    public void PlaySoundFX(AudioFX clip)
    {
        AudioClip fx;
        switch (clip)
        {
            case AudioFX.Intro:
                fx = intro;
                break;
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
            case AudioFX.Mouseover:
                fx = mouseover;
                break;
            default:
                fx = brawl;
                break;
        }

        fxSources[counter].Stop();
        fxSources[counter].clip = fx;
        fxSources[counter].volume = fxVolume;
        fxSources[counter].Play();
        counter = (short)((counter + 1) % sourcesCount);
    }

    public void PlayMusic(Music song)
    {
        AudioClip intro;
        AudioClip loop;
        switch (song)
        {
            case Music.Level1:
                intro = level1Intro;
                loop = level1Loop;
                break;
            default:
                intro = level1Intro;
                loop = level1Loop;
                break;
        }

        musicSource.Stop();
        musicSource.clip = intro;
        musicSource.volume = musicVolume;
        musicSource.Play();
        StartCoroutine(PlayAfter(loop, intro.length));
    }

    private IEnumerator PlayAfter(AudioClip loop, float time)
    {
        yield return new WaitForSeconds(time);
        musicSource.clip = loop;
        musicSource.volume = musicVolume;
        musicSource.pitch = 1.5f;
        musicSource.loop = true;
        musicSource.Play();
    }
}
