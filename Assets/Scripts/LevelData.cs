using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelData : MonoBehaviour
{
	public static LevelData instance;
	public static LevelScene defaultLevel = LevelScene.Level1;

	public LevelScene level;

	public enum LevelScene
	{
		Level1,
		Level2
	}

	private void Awake()
	{
		instance = this;
	}

	private static string SceneToString(LevelScene scene)
	{
		return Enum.GetName(typeof(LevelScene), scene);
	}

	public static void LoadSceneAdditive(LevelScene scene, Action<AsyncOperation> callback)
	{
		SceneManager.LoadSceneAsync(SceneToString(scene), LoadSceneMode.Additive).completed += callback;
	}
}
