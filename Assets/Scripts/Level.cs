using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Level : MonoBehaviour
{
	public Transform background;
	public Transform background2;
	public float levelSpeed;
	public int length;
	public AudioManager.Music levelMusic;

	[SerializeField] private GameObject platform;
	[SerializeField] private Transform levelGeometry;
	private readonly List<Transform> levelPlatforms = new List<Transform>();
	private readonly List<Transform> enemies = new List<Transform>();
	private readonly List<Transform> cannonballs = new List<Transform>();

	[SerializeField] public float initialLevelSpeed = 1.0f;
	[SerializeField] private float yRange = 5f;
	[SerializeField] private Sprite[] levelUpSprites;

	protected bool stuck
	{
		get
		{
			return GameManager.instance.player.GetComponent<Movement>().stuck;
		}
	}

	protected LevelDelegate levelDelegate;

	abstract public void OnGameOver();

	abstract public void OnLevelEnded();

	public virtual void LevelSetup()
	{
		levelSpeed = initialLevelSpeed;

		foreach (Transform p in levelPlatforms)
		{
			Destroy(p.gameObject);
		}

		foreach (Transform e in enemies)
		{
			Destroy(e.gameObject);
		}

		foreach (Transform c in cannonballs)
		{
			Destroy(c.gameObject);
		}

		levelPlatforms.Clear();
		enemies.Clear();
		cannonballs.Clear();

		background.position = new Vector3(0, 0, 69);
		background2.position = new Vector3(100, 0, 69);

		GameManager.instance.player.GetComponent<Movement>().SetSpeed(levelSpeed);
	}

	protected void GenerateNewPlatform(float x = 0, float y = 0, float z = 0, float width = 1f, float height = 1f)
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
		newPlatform.transform.localScale = new Vector3(width, height, 1);
		newPlatform.transform.position = position;
		levelPlatforms.Add(newPlatform.GetComponent<Transform>());

		levelDelegate?.OnPlatformCreated(newPlatform);
	}

	protected void SpawnEnemy(GameObject enemy, Vector3 position)
	{
		GameObject newEnemy = Instantiate(enemy);
		enemies.Add(newEnemy.transform);
		newEnemy.transform.position = position;
	}

	protected void OnSpeedUp(float speedIncrement, bool showLevelUpImage = true)
	{
		levelSpeed += speedIncrement;
		AudioManager.instance.IncreaseMusicSpeed(speedIncrement);

		if (showLevelUpImage)
		{
			//TODO: Move to GameManager
			GameManager.instance.levelUpImage.enabled = true;
			GameManager.instance.levelUpImage.sprite = levelUpSprites[Random.Range(0, levelUpSprites.Length - 1)];
			StartCoroutine(HideLevelUpImage(1));
		}
		GameManager.instance.player.GetComponent<Movement>().SetSpeed(levelSpeed);
	}

	//TODO: Move to GameManager
	private IEnumerator HideLevelUpImage(float after)
	{
		yield return new WaitForSeconds(after);
		GameManager.instance.levelUpImage.enabled = false;
	}

	protected void MovePlatforms()
	{
		if (levelPlatforms.Count == 0)
		{
			return;
		}

		foreach (Transform p in levelPlatforms)
		{
			p.Translate(-levelSpeed, 0, 0);
		}

		if (levelPlatforms[0].position.x < Camera.main.transform.position.x - 80)
		{
			GenerateNewPlatform(0, 0, 0, levelPlatforms[0].localScale.x, levelPlatforms[0].localScale.y);
			Destroy(levelPlatforms[0].gameObject);
			levelPlatforms.RemoveAt(0);
		}
	}

	protected void MoveEnemies()
	{
		if (enemies.Count == 0)
		{
			return;
		}

		foreach (Transform e in enemies)
		{
			e.Translate(-levelSpeed, 0, 0);
		}

		if (enemies[0].position.x < Camera.main.transform.position.x - 80)
		{
			//GenerateNewPlatform(0, 0, 0, levelPlatforms[0].localScale.x, levelPlatforms[0].localScale.y);
			Destroy(enemies[0].gameObject);
			enemies.RemoveAt(0);
		}
	}

	protected void MoveCannonballs()
	{
		if (cannonballs.Count == 0)
		{
			return;
		}

		foreach (Transform c in cannonballs)
		{
			c.Translate(-levelSpeed * c.GetComponent<Cannonball>().relativeSpeedMultiplier, 0, 0);
		}

		if (cannonballs[0].position.x < Camera.main.transform.position.x - 80)
		{
			Destroy(cannonballs[0].gameObject);
			cannonballs.RemoveAt(0);
		}
	}

	protected void SpawnCannonball(GameObject cannonball)
	{
		GameObject newCannonball = Instantiate(cannonball);
		newCannonball.transform.position = new Vector2(GameManager.instance.player.transform.position.x + 25, 30);
		newCannonball.GetComponent<Rigidbody2D>().linearVelocity = Vector2.down * 20;
		newCannonball.GetComponent<Cannonball>().relativeSpeedMultiplier = Random.Range(0.7f, 0.9f);
		cannonballs.Add(newCannonball.transform);
	}

	public void DestroyCannonball(GameObject cannonball)
	{
		cannonballs.Remove(cannonball.transform);
		Destroy(cannonball.gameObject);
	}
}


public interface LevelDelegate
{
	void OnPlatformCreated(GameObject platform);
}