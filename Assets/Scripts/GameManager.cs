using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject player;
    private Transform playerTransform;
    [SerializeField] private float killFloor = -10;
    [SerializeField] private CameraFollow cameraFollow;

    [SerializeField] public float parallaxMultiplier = 1.0f;
    public bool paused = true;

    [Header("UI")]
    [SerializeField] private Canvas gameUI;
    [SerializeField] private Canvas gameOverScreen;
    [SerializeField] private Text scoreLabel;
    [SerializeField] private Text gameOverScoreLabel;
    [SerializeField] private Text gameOverHighScoreLabel;
    public int score = 0;
    [SerializeField] private SpriteRenderer startCounter;
    [SerializeField] private Sprite one;
    [SerializeField] private Sprite two;
    [SerializeField] private Sprite three;
    [SerializeField] private Sprite four;
    [SerializeField] private Sprite five;
    [SerializeField] private Sprite brawl;


    [SerializeField] private Level level;
    [SerializeField] private Slider levelProgress;

    void Start()
    {
        instance = this;
        playerTransform = player.GetComponent<Transform>();
        //StartCoroutine(GenerateLoop());
        if(AudioManager.instance == null)
        {
            AudioManager.instance = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        }
        AudioManager.instance.PlayMusic(level.levelMusic);
        LevelSetup();
    }

    private void LevelSetup()
    {
        paused = true;

        scoreLabel.text = "Score: 0";
        score = 0;

        playerTransform.position = new Vector3(0, 0, 0);
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        gameOverScreen.enabled = false;
        gameUI.enabled = true;

        level.LevelSetup();

        if(level.length > 0) //If length <= 0, it's an endless level
        {
            levelProgress.gameObject.SetActive(true);
            levelProgress.maxValue = level.length;
            levelProgress.value = 0;
        }
        else
        {
            levelProgress.gameObject.SetActive(false);
        }

        StartCoroutine(Countdown());
    }

    void FixedUpdate()
    {
        if (paused)
        {
            return;
        }

        if(playerTransform.position.y < killFloor)
        {
            OnPlayerFell();
            return;
        }

        level.background.Translate(-level.levelSpeed * parallaxMultiplier, 0, 0);
        level.background2.Translate(-level.levelSpeed * parallaxMultiplier, 0, 0);

        if (level.background.position.x < Camera.main.transform.position.x - 80)
        {
            level.background.transform.position = new Vector3(level.background2.transform.position.x + 100, 0, 69);
            Transform tmp = level.background;
            level.background = level.background2;
            level.background2 = tmp;
        }

        score += 1;
        scoreLabel.text = $"Score: {score/10}";

        if (level.length > 0)
        {
            levelProgress.value = score / 10f;
            if(score/10f >= level.length)
            {
                level.OnLevelEnded();
            }
        }
    }

    private void OnPlayerFell()
    {
        cameraFollow.enabled = false;
        paused = true;
        gameUI.enabled = false;
        gameOverScreen.enabled = true;
        levelProgress.gameObject.SetActive(false);
        gameOverScoreLabel.text = $"Score: {score / 10}";
        AudioManager.instance.PlayDeathSound();

        level.OnGameOver();

        if (!PlayerPrefs.HasKey("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore", score / 10);
            PlayerPrefs.Save();
            gameOverHighScoreLabel.enabled = false;
        }
        else
        {
            int highScore = PlayerPrefs.GetInt("HighScore");
            if(score / 10 > highScore)
            {
                highScore = score / 10;
                PlayerPrefs.SetInt("HighScore", highScore);
                PlayerPrefs.Save();
            }
            gameOverHighScoreLabel.enabled = true;
            gameOverHighScoreLabel.text = $"High Score: {highScore}";
        }

    }

    private IEnumerator Countdown()
    {
        startCounter.enabled = true;

        if (Random.Range(1, 10) <= 7) // expected value = 0.2 (chance of wrong countdown happening)
        {
            for(short i = 3; i >= 1; i--)
            {
                CounterNumber(i);
                yield return new WaitForSeconds(1);
            }
        }
        else
        {
            short times = (short) Random.Range(2, 6);
            //while(Random.Range(1, 10) <= 8) // expected value = 5 (number of numbers displayed)
            for(short i = 0; i < times; i++)
            {
                CounterNumber((short) Random.Range(1, 5));
                yield return new WaitForSeconds(1);
            }
        }
        AudioManager.instance.PlaySoundFX(AudioManager.AudioFX.Brawl);
        startCounter.sprite = brawl;
        yield return new WaitForSeconds(1);
        startCounter.enabled = false;
        paused = false;
    }

    private void CounterNumber(short n)
    {
        AudioManager.AudioFX clip;
        Sprite sprite;
        switch (n)
        {
            case 5:
                clip = AudioManager.AudioFX.Five;
                sprite = five;
                break;
            case 4:
                clip = AudioManager.AudioFX.Four;
                sprite = four;
                break;
            case 3:
                clip = AudioManager.AudioFX.Three;
                sprite = three;
                break;
            case 2:
                clip = AudioManager.AudioFX.Two;
                sprite = two;
                break;
            case 1:
                clip = AudioManager.AudioFX.One;
                sprite = one;
                break;
            default:
                clip = AudioManager.AudioFX.Three;
                sprite = three;
                break;
        }
        AudioManager.instance.PlaySoundFX(clip);
        startCounter.sprite = sprite;
    }

    public void OnQuitClicked()
    {
        Application.Quit();
    }

    public void OnRetryClicked()
    {
        LevelSetup();
        AudioManager.instance.ResetMusicSpeed();
        cameraFollow.enabled = true;
    }

    public void OnRetryHovered()
    {
        AudioManager.instance.PlaySoundFX(AudioManager.AudioFX.Continue);
    }

}
