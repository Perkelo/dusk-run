using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
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
		Mouseover,
		Continue,
		TerosBuildup,
		TerosGrowl,
		HammerImpact,
		Jump,
		GroundTouch,
		Win
	}

	public enum Music
	{
		Level1,
		Shipwreck
	}

	[HideInInspector] public static AudioManager instance;
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
	[SerializeField] private AudioClip announcerContinue;
	[SerializeField] private AudioClip terosBuildup;
	[SerializeField] private AudioClip[] terosGrowls;
	[SerializeField] private AudioClip hammerImpact;
	[SerializeField] private AudioClip[] jumpSounds;
	[SerializeField] private AudioClip groundTouch;
	[SerializeField] private AudioClip[] winSounds;
	[Header("Music")]
	[SerializeField] private AudioClip level1Intro;
	[SerializeField] private AudioClip level1Loop;
	[SerializeField] private AudioClip shipwreckIntro;
	[SerializeField] private AudioClip shipwreckLoop;
	private short counter = 0;
	[Header("Death Sounds")]
	[SerializeField] private AudioClip[] deathSounds;

	private void Awake()
	{
		if (instance != this && instance != null)
		{
			Destroy(this);
			return;
		}
		instance = this;

		fxSources = new AudioSource[sourcesCount];
		for (short i = 0; i < sourcesCount; i++)
		{
			fxSources[i] = gameObject.AddComponent<AudioSource>();
		}
		musicSource = gameObject.AddComponent<AudioSource>();
		DontDestroyOnLoad(this);
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
			case AudioFX.Continue:
				fx = announcerContinue;
				break;
			case AudioFX.TerosBuildup:
				fx = terosBuildup;
				break;
			case AudioFX.TerosGrowl:
				fx = terosGrowls[Random.Range(0, terosGrowls.Length - 1)];
				break;
			case AudioFX.HammerImpact:
				fx = hammerImpact;
				break;
			case AudioFX.Jump:
				fx = jumpSounds[Random.Range(0, jumpSounds.Length - 1)];
				break;
			case AudioFX.GroundTouch:
				fx = groundTouch;
				break;
			case AudioFX.Win:
				fx = winSounds[Random.Range(0, winSounds.Length - 1)];
				break;
			default:
				fx = brawl;
				break;
		}

		short iter = (short)fxSources.Length; //Can cast without problems because the length is bound by sourcesCount
		while (fxSources[counter].isPlaying && iter >= 0)
		{
			counter = (short)((counter + 1) % sourcesCount);
			iter--;
		}
		if (iter <= 0)
		{ //No free audiosource has been found
			Debug.Log("Trying to play soundFX while all sources are busy");
			return;
		}

		fxSources[counter].Stop();
		fxSources[counter].clip = fx;
		fxSources[counter].volume = fxVolume;
		fxSources[counter].Play();
	}

	public void PlayMusic(Music song, float pitch = 1f)
	{
		AudioClip intro;
		AudioClip loop;
		switch (song)
		{
			case Music.Level1:
				intro = level1Intro;
				loop = level1Loop;
				break;
			case Music.Shipwreck:
				intro = shipwreckIntro;
				loop = shipwreckLoop;
				break;
			default:
				intro = level1Intro;
				loop = level1Loop;
				break;
		}

		musicSource.Stop();
		musicSource.pitch = 1.0f;
		musicSource.clip = intro;
		musicSource.volume = musicVolume;
		musicSource.Play();
		StartCoroutine(PlayAfter(loop, intro.length, pitch));
	}

	public void StopMusic()
	{
		musicSource.Stop();
	}

	private IEnumerator PlayAfter(AudioClip loop, float time, float pitch)
	{
		yield return new WaitForSeconds(time);
		musicSource.clip = loop;
		musicSource.volume = musicVolume;
		musicSource.pitch = 1.5f;
		musicSource.loop = true;
		musicSource.Play();
	}

	public void PlayDeathSound()
	{
		fxSources[counter].Stop();
		fxSources[counter].clip = deathSounds[Random.Range(0, deathSounds.Length - 1)];
		fxSources[counter].volume = fxVolume;
		fxSources[counter].Play();
		counter = (short)((counter + 1) % sourcesCount);
	}

	public void ChangeMusicSpeed(float newSpeed, float time = 0.5f)
	{
		StartCoroutine(ChangeMusicSpeedCoroutine(newSpeed, time));
	}
	public void IncreaseMusicSpeed(float increment, float time = 0.5f)
	{
		ChangeMusicSpeed(musicSource.pitch + increment, time);
	}

	private IEnumerator ChangeMusicSpeedCoroutine(float newSpeed, float time)
	{
		float elapsedTime = 0;
		float initialSpeed = musicSource.pitch;
		while (time - elapsedTime > 0)
		{
			musicSource.pitch = Mathf.Lerp(initialSpeed, newSpeed, elapsedTime / time);
			elapsedTime += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
	}

	public void ResetMusicSpeed(float time = 0.5f)
	{
		ChangeMusicSpeed(1.5f, time);
	}
}
