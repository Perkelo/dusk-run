using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;

public class PlayerHealth : MonoBehaviour
{
	[SerializeField] private int health = 3;
	private bool canBeHit = true;
	[SerializeField] private float invincibilityTime = 1f;

	public bool Hit(int damage = 1)
	{
		if (canBeHit) {
			health -= damage;
			canBeHit = false;
			this.RunAfter(invincibilityTime, delegate
			{
				canBeHit = true;
			});
			GameManager.instance.RemoveHearts(health);
			//Debug.Log("Hit");
			if (health <= 0)
			{
				GameManager.instance.HealthGameOver();
			}
			return true;
		}
		else
		{
			return false;
		}
	}

	public void InitializeHealth(int health)
	{
		this.health = health;
	}
}
