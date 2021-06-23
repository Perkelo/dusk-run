using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    private Transform playerTransform;
    [SerializeField] private float killFloor = -10;

    [SerializeField] private Transform background;
    [SerializeField] private Transform background2;
    [SerializeField] private GameObject platform;
    [SerializeField] private Transform levelGeometry;
    private List<Transform> levelPlatforms = new List<Transform>();
    [SerializeField] public float levelSpeed = 1.0f;
    [SerializeField] public float parallaxMultiplier = 1.0f;
    [SerializeField] private float spawnDelay = 1f;
    [SerializeField] private float yRange = 5f;

    void Start()
    {
        playerTransform = player.GetComponent<Transform>();
        levelPlatforms.Add(GameObject.Find("Floor").GetComponent<Transform>());
        //StartCoroutine(GenerateLoop());
        LevelSetup();
    }

    private void LevelSetup()
    {
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
    }


    void FixedUpdate()
    {
        if(playerTransform.position.y < killFloor)
        {
            OnPlayerFell();
            return;
        }

        foreach(Transform p in levelPlatforms)
        {
            p.Translate(-levelSpeed, 0, 0);
        }
        background.Translate(-levelSpeed * parallaxMultiplier, 0, 0);
        background2.Translate(-levelSpeed * parallaxMultiplier, 0, 0);

        if (background.position.x < -80)
        {
            background.transform.position = new Vector3(100, 0, 69);
            Transform tmp = background;
            background = background2;
            background2 = tmp;
        }

        if(levelPlatforms[0].position.x < -80)
        {
            Destroy(levelPlatforms[0].gameObject);
            levelPlatforms.RemoveAt(0);
            GenerateNewPlatform();
        }
    }

    private void OnPlayerFell()
    {
        LevelSetup();
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
            position = new Vector3(levelPlatforms[levelPlatforms.Count - 1].position.x + 20, Random.Range(-yRange, yRange), levelPlatforms[levelPlatforms.Count - 1].position.z);
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
}
