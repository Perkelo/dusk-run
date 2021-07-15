using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
	private Rigidbody2D rb2d;
	private SpriteRenderer spriteRenderer;
	[SerializeField] private ContactFilter2D cf;
	[SerializeField] private float speed = 1f;
	[SerializeField] private float aerialSpeed = 1f;
	[SerializeField] private float turnaroundSpeed = 0.5f;
	[SerializeField] private float jumpForce = 1f;
	[SerializeField] private float jumpFloorDistance = 1f;
	[SerializeField] private float stuckDistance = 1f;
	[SerializeField] private float normalFallingMaxSpeed = Mathf.Infinity;
	[SerializeField] private float fastFallingMaxSpeed = Mathf.Infinity;
	[SerializeField] private float fastFallInitialForce = 1f;
	private Vector2 movement = Vector2.zero;
	[SerializeField] private bool isFastFalling = false;
	[SerializeField] private short maxJumps = 3;
	[SerializeField]  private short jumpCounter = 0;

	[SerializeField] private bool grounded = false;
	[SerializeField] public bool stuck = false;
	public bool lrMovementDisabled = true;
	public bool movementDisabled = false;

	void Start()
	{
		rb2d = GetComponent<Rigidbody2D>();
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	private void OnGroundTouch()
	{
		AudioManager.instance.PlaySoundFX(AudioManager.AudioFX.GroundTouch);
		grounded = true;
		isFastFalling = false;
		jumpCounter = 0;
	}

	private void FixedUpdate()
	{
		if(movementDisabled){
			return;
		}
		GroundCheck();
		StuckCheck();

		//Debug.Log(movement);
		//rb2d.AddForce(movement * speed);
		Vector2 newVelocity = Vector2.Lerp(rb2d.velocity, new Vector2(movement.x * speed, rb2d.velocity.y), (grounded ? turnaroundSpeed : aerialSpeed));
		newVelocity.y = Mathf.Max(newVelocity.y, isFastFalling ? -fastFallingMaxSpeed : -normalFallingMaxSpeed);
		//newVelocity.x = 0;
		spriteRenderer.flipX = newVelocity.x > 0;

		rb2d.velocity = newVelocity;
		//Debug.Log(rb2d.velocity);
	}

	private void GroundCheck()
	{
		List<RaycastHit2D> res = new List<RaycastHit2D>();
		if (Physics2D.Raycast(transform.position, Vector2.down, cf, res, jumpFloorDistance) > 0)
		{
			bool touched = false;
			foreach (RaycastHit2D r in res)
			{
				if (r.collider.name != "Player")
				{
					touched = true;
					if (!grounded)
					{
						OnGroundTouch();
					}
					return;
				}
			}
			if (!touched)
			{
				grounded = false;
			}
		}
	}

	private void StuckCheck()
	{
		List<RaycastHit2D> res = new List<RaycastHit2D>();
		if (Physics2D.Raycast(transform.position, Vector2.right, cf, res, stuckDistance) > 0)
		{
			bool touched = false;
			foreach (RaycastHit2D r in res)
			{
				if (r.collider.name != "Player")
				{
					touched = true;
					stuck = true;
					return;
				}
			}
			if (!touched)
			{
				stuck = false;
			}
		}
	}

	public void Move(InputAction.CallbackContext context)
	{
		if (lrMovementDisabled)
		{
			return;
		}
		if (!context.canceled)
		{
			movement.x = context.ReadValue<Vector2>().normalized.x;
			//Debug.Log(movement.x);
		}
		else
		{
			movement = Vector2.zero;
		}
		GameManager.instance.currentPlayerSpeedMultiplier = 1 + (movement.x / 2f);
	}

	public void Jump(InputAction.CallbackContext context)
	{
		if (context.started)
		{
			if (grounded || jumpCounter < maxJumps)
			{
				jumpCounter++;
				//rb2d.velocity = new Vector2(0, rb2d.velocity.x);
				//rb2d.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
				rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
				AudioManager.instance.PlaySoundFX(AudioManager.AudioFX.Jump);
			}
		}
	}

	public void FastFall(InputAction.CallbackContext context)
	{
		if (context.started)
		{
			if (!grounded)
			{
				isFastFalling = true;
				Vector2 newVelocity = rb2d.velocity;
				newVelocity.y = /*Mathf.Max(newVelocity.y, 0)*/ - fastFallInitialForce;
				rb2d.velocity = newVelocity;
			}
		}
		else if (context.canceled)
		{
			isFastFalling = false;
		}
	}

	public void Quit(InputAction.CallbackContext context)
	{
		Application.Quit();
	}

	public void SetSpeed(float levelSpeed) {
		this.speed = levelSpeed * 20;
	}
}
