using System.Collections;
using System.Collections.Generic;
using Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelShipwreck : Level, LevelDelegate
{
	[SerializeField][Range(0, 1)] private float terosSpawningProbability = 0.2f;
	[SerializeField] private GameObject teros;

	[SerializeField] private GameObject cannonball;

	public override void LevelSetup()
	{
		levelDelegate = null;
		base.LevelSetup();
		GenerateNewPlatform(0, -4, 0, 1.4f, 1);
		GenerateNewPlatform(0, 0, 0, 1.4f, 1);
		GenerateNewPlatform(0, 0, 0, 1.4f, 1);
		GenerateNewPlatform(0, 0, 0, 1.4f, 1);
		GenerateNewPlatform(0, 0, 0, 1.4f, 1);
		GenerateNewPlatform(0, 0, 0, 1.4f, 1);
		GenerateNewPlatform(0, 0, 0, 1.4f, 1);
		GenerateNewPlatform(0, 0, 0, 1.4f, 1);
		GenerateNewPlatform(0, 0, 0, 1.4f, 1);
		levelDelegate = this;
	}

	public override void OnGameOver()
	{
		GameManager.instance.warning.enabled = false;
	}

	public override void OnLevelEnded()
	{
		GameManager.instance.paused = true;

		AudioManager.instance.PlaySoundFX(AudioManager.AudioFX.Win);

		this.RunAfter(0.5f, delegate
		{
			GameManager.instance.DisplayLore("The end", "This is the end of the currently developed game", delegate
			{
				AudioManager.instance.StopMusic();
				SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
			});
			LevelData.UnloadScene(LevelData.LevelScene.Level2, delegate { });
		});
	}

	public void OnPlatformCreated(GameObject platform)
	{
		if (Extensions.Extensions.RandomBool(terosSpawningProbability))
		{
			Vector3 extents = platform.GetComponent<SpriteRenderer>().bounds.extents;
			//Debug.Log(extents);
			SpawnEnemy(teros, new Vector3(
				platform.transform.position.x + Random.Range(-extents.x, extents.x),
				platform.transform.position.y + extents.y + 1,
				0));
		}
		else
		{
			//SpawnCannonball(cannonball);
		}
	}

	private void FixedUpdate()
	{
		if (GameManager.instance.paused)
		{
			return;
		}

		if (stuck)
		{
			return;
		}

		MovePlatforms();
		MoveEnemies();
		MoveCannonballs();
	}
}
