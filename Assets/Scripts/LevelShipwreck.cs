using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelShipwreck : Level
{
    public override void LevelSetup()
    {
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
    }

    public override void OnGameOver()
    {
        throw new System.NotImplementedException();
    }

    public override void OnLevelEnded()
    {
        throw new System.NotImplementedException();
    }

    private void FixedUpdate()
    {
        if (GameManager.instance.paused)
        {
            return;
        }

        MovePlatforms();
    }
}
