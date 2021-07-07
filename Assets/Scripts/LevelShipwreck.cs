using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelShipwreck : Level, LevelDelegate
{
	[SerializeField] [Range(0, 1)] private float terosSpawningProbability = 0.2f;
	[SerializeField] private GameObject teros;

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
		throw new System.NotImplementedException();
	}

	public override void OnLevelEnded()
	{
		throw new System.NotImplementedException();
	}

	public void OnPlatformCreated(GameObject platform)
	{
		if (Extensions.Extensions.RandomBool(terosSpawningProbability))
		{
			Vector3 extents = platform.GetComponent<SpriteRenderer>().bounds.extents;
			Debug.Log(extents);
			SpawnEnemy(teros, new Vector3(
				platform.transform.position.x + Random.Range(-extents.x, extents.x),
				platform.transform.position.y + extents.y + 1,
				0));
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
	}
}
