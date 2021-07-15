using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannonball : MonoBehaviour
{
	[SerializeField] private float knockbackForce = 1f;
	public float relativeSpeedMultiplier = 0.9f;

	private void OnCollisionEnter2D(Collision2D collision) {
		if(collision.gameObject.CompareTag("Player")) {
			if(collision.gameObject.GetComponent<PlayerHealth>().Hit()) {
				collision.gameObject.GetComponent<Rigidbody2D>().AddForce((collision.gameObject.transform.position - collision.otherCollider.transform.position) * knockbackForce, ForceMode2D.Impulse);
				AudioManager.instance.PlaySoundFX(AudioManager.AudioFX.HammerImpact);
			}
		} else if(collision.gameObject.CompareTag("Level")){
			//Explode();
		} else {
			//Debug.Log(collision.gameObject.name);
			//Debug.Log(collision.gameObject.tag);
		}
		Explode();
	}

	private void Explode() {
		GameManager.instance.level.DestroyCannonball(this.gameObject);
	}
}
