using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour{
	[SerializeField] private Transform objectToFollow;
	private Vector3 speed = Vector3.zero;
	[SerializeField] public float smoothness = 1;
	[SerializeField] public float offsetX = 3;
	[SerializeField] public float offsetY = 3;

	void FixedUpdate(){
		if (objectToFollow != null)
		{
			Vector3 target = new Vector3(objectToFollow.position.x + offsetX, objectToFollow.position.y + offsetY, -10);
			transform.position = Vector3.SmoothDamp(transform.position, target, ref speed, smoothness);
		}
	}
}
