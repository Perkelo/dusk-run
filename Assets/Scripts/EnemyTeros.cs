using UnityEngine;
using Extensions;

public class EnemyTeros : MonoBehaviour
{
	private Rigidbody2D rb2d;
	[SerializeField] private float attackForce = 1f;
	[SerializeField] private float minimumDistance = 10f;
	[SerializeField] private float knockbackForce = 15f;

	private bool shouldCheckForPlayer = true;

	private Transform playerTransform;

	void Start()
	{
		rb2d = GetComponent<Rigidbody2D>();
		playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
	}

	private void FixedUpdate()
	{
		if (shouldCheckForPlayer)
		{
			if(Mathf.Abs(transform.position.x - playerTransform.position.x) < minimumDistance)
			{
				shouldCheckForPlayer = false;
				Attack();
			}
			else
			{
				shouldCheckForPlayer = false;
				this.RunAfter(0.5f, delegate
				{
					shouldCheckForPlayer = true;
				});
			}
		}
	}

	private void Attack()
	{
		AudioManager.instance.PlaySoundFX(AudioManager.AudioFX.TerosBuildup);
		this.RunAfter(0.5f, delegate
		{
			rb2d.AddForce(Vector2.left * attackForce, ForceMode2D.Impulse);
			AudioManager.instance.PlaySoundFX(AudioManager.AudioFX.TerosGrowl);

			//this.RunAfter(3f, delegate { Attack(); });
		});
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if(collision.gameObject.CompareTag("Player"))
		{
			if (collision.gameObject.GetComponent<PlayerHealth>().Hit())
			{
				collision.gameObject.GetComponent<Rigidbody2D>().AddForce((collision.gameObject.transform.position - collision.otherCollider.transform.position) * knockbackForce, ForceMode2D.Impulse);
				AudioManager.instance.PlaySoundFX(AudioManager.AudioFX.HammerImpact);
			}
		}
		else
		{
			//Debug.Log(collision.gameObject.name);
		}
	}
}
