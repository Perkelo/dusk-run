using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelBrawlhaven : Level
{
    [SerializeField] private GameObject platform;
    [SerializeField] private Transform levelGeometry;
    private readonly List<Transform> levelPlatforms = new List<Transform>();

    [SerializeField] public float initialLevelSpeed = 1.0f;
    [SerializeField] private Image warning;
    [SerializeField] private Image levelUpImage;
    [SerializeField] private Sprite[] levelUpSprites;
    //[SerializeField] private float spawnDelay = 1f;
    [SerializeField] private float yRange = 5f;

    public override void LevelSetup()
    {
        levelSpeed = initialLevelSpeed;

        foreach (Transform p in levelPlatforms)
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

        background.position = new Vector3(0, 0, 69);
        background2.position = new Vector3(100, 0, 69);
    }

    public override void OnGameOver()
    {
        warning.enabled = false;
    }

    public override void OnLevelEnded()
    {
        throw new System.NotImplementedException();
    }

    private void GenerateNewPlatform(float x = 0, float y = 0, float z = 0)
    {
        GameObject newPlatform = Instantiate(platform, levelGeometry);
        Vector3 position;
        if (x != 0 || y != 0 || z != 0)
        {
            position = new Vector3(x, y, z);
        }
        else
        {
            position = new Vector3(levelPlatforms[levelPlatforms.Count - 1].position.x + 20 + GameManager.instance.score / 500, Random.Range(-yRange, yRange), levelPlatforms[levelPlatforms.Count - 1].position.z);
        }
        newPlatform.transform.position = position;
        levelPlatforms.Add(newPlatform.GetComponent<Transform>());
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

    private void FixedUpdate()
    {
        if (GameManager.instance.paused)
        {
            return;
        }
        CheckSpeedIncrease();
        MovePlatforms();
    }

    private void CheckSpeedIncrease()
    {
        int scoremod = GameManager.instance.score % 1000;

        if (scoremod == 0 && GameManager.instance.score != 0)
        {
            OnSpeedUp();
        }
        else if (scoremod == 850 || scoremod == 900 || scoremod == 950)
        {
            warning.enabled = true;
        }
        else if (scoremod == 875 || scoremod == 925 || scoremod == 975)
        {
            warning.enabled = false;
        }
    }

    private void MovePlatforms()
    {
        foreach (Transform p in levelPlatforms)
        {
            p.Translate(-levelSpeed, 0, 0);
        }

        if (levelPlatforms[0].position.x < Camera.main.transform.position.x - 80)
        {
            Destroy(levelPlatforms[0].gameObject);
            levelPlatforms.RemoveAt(0);
            GenerateNewPlatform();
        }
    }
}
