using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    private Transform playerTransform;
    [SerializeField] private float killFloor = -10;
    [SerializeField] private CameraFollow cameraFollow;

    [SerializeField] private Transform background;
    [SerializeField] private Transform background2;
    [SerializeField] private GameObject platform;
    [SerializeField] private Transform levelGeometry;
    private List<Transform> levelPlatforms = new List<Transform>();
    [SerializeField] public float levelSpeed = 1.0f;
    [SerializeField] public float initialLevelSpeed = 1.0f;
    [SerializeField] public float parallaxMultiplier = 1.0f;
    [SerializeField] private float spawnDelay = 1f;
    [SerializeField] private float yRange = 5f;
    [SerializeField] private bool paused = true;

    [Header("UI")]
    [SerializeField] private Canvas gameUI;
    [SerializeField] private Canvas gameOverScreen;
    [SerializeField] private Text scoreLabel;
    [SerializeField] private Text gameOverScoreLabel;
    [SerializeField] private Text gameOverHighScoreLabel;
    [SerializeField] private Image warning;
    [SerializeField] private Image levelUpImage;
    [SerializeField] private Sprite[] levelUpSprites;
    private int score = 0;
    [SerializeField] private SpriteRenderer startCounter;
    [SerializeField] private Sprite one;
    [SerializeField] private Sprite two;
    [SerializeField] private Sprite three;
    [SerializeField] private Sprite four;
    [SerializeField] private Sprite five;
    [SerializeField] private Sprite brawl;

    void Start()
    {
        playerTransform = player.GetComponent<Transform>();
        //StartCoroutine(GenerateLoop());
        if(AudioManager.instance == null)
        {
            AudioManager.instance = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        }
        AudioManager.instance.PlayMusic(AudioManager.Music.Level1);
        LevelSetup();
    }

    private void LevelSetup()
    {
        paused = true;

        scoreLabel.text = "Score: 0";
        score = 0;

        levelSpeed = initialLevelSpeed;

        foreach(Transform p in levelPlatforms)
        {
            Destroy(p.gameObject);
        }
        //levelPlatforms.RemoveRange(0, levelPlatforms.Count - 1);

        levelPlatforms.Clear();

        GenerateNewPlatform(0, -4, 0);
        GenerateNewPlatform();
        GenerateNewPlatform();
        GenerateNewPlatform();
        GenerateNewPlatform();
        GenerateNewPlatform();
        GenerateNewPlatform();

        playerTransform.position = new Vector3(0, 0, 0);
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        background.position = new Vector3(0, 0, 69);
        background2.position = new Vector3(100, 0, 69);

        gameOverScreen.enabled = false;
        gameUI.enabled = true;

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

        foreach(Transform p in levelPlatforms)
        {
            p.Translate(-levelSpeed, 0, 0);
        }

        score += 1;
        scoreLabel.text = $"Score: {score/10}";

        int scoremod = score % 1000;

        if (scoremod == 0 && score != 0)
        {
            OnSpeedUp();
        }
        else if(scoremod == 850 || scoremod == 900 || scoremod == 950)
        {
            warning.enabled = true;
        }
        else if(scoremod == 875 || scoremod == 925 || scoremod == 975)
        {
            warning.enabled = false;
        }

        background.Translate(-levelSpeed * parallaxMultiplier, 0, 0);
        background2.Translate(-levelSpeed * parallaxMultiplier, 0, 0);

        if (background.position.x < Camera.main.transform.position.x - 80)
        {
            background.transform.position = new Vector3(background2.transform.position.x + 100, 0, 69);
            Transform tmp = background;
            background = background2;
            background2 = tmp;
        }

        if(levelPlatforms[0].position.x < Camera.main.transform.position.x - 80)
        {
            Destroy(levelPlatforms[0].gameObject);
            levelPlatforms.RemoveAt(0);
            GenerateNewPlatform();
        }
    }

    private void OnPlayerFell()
    {
        cameraFollow.enabled = false;
        paused = true;
        warning.enabled = false;
        gameUI.enabled = false;
        gameOverScreen.enabled = true;
        gameOverScoreLabel.text = $"Score: {score / 10}";
        AudioManager.instance.PlayDeathSound();

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

    private void GenerateNewPlatform(float x = 0, float y = 0, float z = 0)
    {
        GameObject newPlatform = Instantiate(platform, levelGeometry);
        Vector3 position;
        if(x != 0 || y != 0 || z != 0)
        {
            position = new Vector3(x, y, z);
        }
        else
        {
            position = new Vector3(levelPlatforms[levelPlatforms.Count - 1].position.x + 20 + score/500, Random.Range(-yRange, yRange), levelPlatforms[levelPlatforms.Count - 1].position.z);
        }
        newPlatform.transform.position = position;
        levelPlatforms.Add(newPlatform.GetComponent<Transform>());
    }

    private IEnumerator GenerateLoop()
    {
        GenerateNewPlatform();
        GenerateNewPlatform();
        GenerateNewPlatform();
        while (true)
        {
            GenerateNewPlatform();
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    private IEnumerator UnpauseAfter(float time)
    {
        yield return new WaitForSeconds(time);
        paused = false;
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

    private void OnSpeedUp()
    {
        levelSpeed += 0.2f;
        AudioManager.instance.IncreaseMusicSpeed(0.2f);

        levelUpImage.enabled = true;
        levelUpImage.sprite = levelUpSprites[Random.Range(0, levelUpSprites.Length - 1)];
        StartCoroutine(HideLevelUpImage(1));
    }

    private IEnumerator HideLevelUpImage(float after)
    {
        yield return new WaitForSeconds(after);
        levelUpImage.enabled = false;
    }
}
