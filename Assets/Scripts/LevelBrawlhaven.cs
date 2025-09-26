using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Extensions;

public class LevelBrawlhaven : Level
{

	public override void LevelSetup()
	{
		base.LevelSetup();
		GenerateNewPlatform(0, -4, 0, 0.7f, 0.7f);
		GenerateNewPlatform(0, 0, 0, 0.7f, 0.7f);
		GenerateNewPlatform(0, 0, 0, 0.7f, 0.7f);
		GenerateNewPlatform(0, 0, 0, 0.7f, 0.7f);
		GenerateNewPlatform(0, 0, 0, 0.7f, 0.7f);
		GenerateNewPlatform(0, 0, 0, 0.7f, 0.7f);
		GenerateNewPlatform(0, 0, 0, 0.7f, 0.7f);
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
			GameManager.instance.DisplayLore("Second Level", "This is the story for the second level.", delegate
			{
				LevelData.instance.level = LevelData.LevelScene.Level2;
				GameManager.instance.LoadLevel();
			});
			LevelData.UnloadScene(LevelData.LevelScene.Level1, delegate { });
		});
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

		CheckSpeedIncrease();
		MovePlatforms();
	}

	private void CheckSpeedIncrease()
	{
		int scoremod = (int)GameManager.instance.score % 1000;

		if (scoremod == 0 && GameManager.instance.score != 0)
		{
			OnSpeedUp(0.2f);
		}
		else if (scoremod == 850 || scoremod == 900 || scoremod == 950)
		{
			GameManager.instance.warning.enabled = true;
		}
		else if (scoremod == 875 || scoremod == 925 || scoremod == 975)
		{
			GameManager.instance.warning.enabled = false;
		}
	}
}
