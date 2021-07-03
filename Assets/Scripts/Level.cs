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

    abstract public void LevelSetup();
    abstract public void OnGameOver();

    abstract public void OnLevelEnded();
}
