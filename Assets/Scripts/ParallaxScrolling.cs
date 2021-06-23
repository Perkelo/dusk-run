using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxScrolling : MonoBehaviour
{

    private float initialZ;
    [SerializeField] private float pFactorX = 1.0f;
    [SerializeField] private float pFactorY = 1.0f;

    void Start()
    {
        initialZ = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, (Camera.main.transform.position.y) * pFactorY, initialZ);
    }
}
