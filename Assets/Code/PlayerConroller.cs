using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerConroller : MonoBehaviour
{
	public Rigidbody2D rbBody { get; set; }
	private Rigidbody2D child1;
	private Rigidbody2D child2;
	public string inputHorizontal;
	public string inputVertical;

	public float horizontalForce;
	public int inputBlocked { get; private set; } = 0;
	public float verticalForce;
	public float jumpForceMultiplier = 1.0f; 

	private float maxVelocity = 30;
	public float impulseMultiplier = 5.0f;
	public float currentHorizontalForce;
	public float currentVerticalForce;

	public float crashMultiplier = 20.0f;
	public float floorHitDistance = 1.0f;

	public Vector2 hitDir = Vector2.up;
	public LayerMask groundLayerMask;

	public List<PlayerConroller> enemies = new List<PlayerConroller>();
	public List<LineRenderer> lines = new List<LineRenderer>();
	private System.Random rng = new System.Random();
	private CrashForce crashForce;

	public bool isGrounded = true;
	public bool crashing;
	private float explosionForce = 50;

	public float deathPosition = -15;
	private CharacterSpawner cSpawner;
	public int HP { get; set; } = 5;

	private void Awake()
	{
		cSpawner = FindObjectOfType<CharacterSpawner>();
		rbBody = transform.Find("Body").GetComponent<Rigidbody2D>();
		child1 = rbBody.transform.Find("Arm_R").GetChild(3).GetComponent<Rigidbody2D>();
		child2 = rbBody.transform.Find("Arm_L").GetChild(3).GetComponent<Rigidbody2D>();
		enemies = FindObjectsOfType<PlayerConroller>().Except(this.Yield()).ToList();
		foreach(Transform ch in rbBody.transform)
		{
			var lr = ch.GetComponent<LineRenderer>();
			if(lr != null)
			{
				lines.Add(lr);
			}
		}
		crashForce = rbBody.GetComponent<CrashForce>();
	}

	internal void MultiplyRayLine(float growMultiplier)
	{
		floorHitDistance *= growMultiplier;
	}

	internal float GetExplosionForce()
	{
		var force = explosionForce * crashBonus * rbBody.transform.localScale.x;
		return force;
	}

	internal void LoseInput(float v)
	{
		inputBlocked++;
		PlayDizzyEffect();
		StartCoroutine(RegainInput(v));
	}

	private void PlayDizzyEffect()
	{
		
	}

	private IEnumerator RegainInput(float seconds)
	{
		yield return new WaitForSeconds(seconds);
		inputBlocked--;
	}

	private IEnumerable<PlayerConroller> Yield()
	{
		yield return this;
	}

	public bool alive = true;
	void Update()
    {		
		if(inputBlocked == 0)
		{
			HandleMovement();
			HandleRayCast();
		}		
		if(rbBody.transform.position.y < deathPosition && alive)
		{
			cSpawner.OnDeath?.Invoke(rbBody.transform);
			alive = false;
			HP--;
		}
	}

	public float GetCurrentHorizontalForce()
	{
		return currentHorizontalForce;
	}

	public void MultiplyScale(float multiplier)
	{
		rbBody.transform.localScale *= multiplier;
	}

	public void MultiplyLines(float multiplier)
	{
		foreach(var line in lines)
		{
			line.SetWidth(line.startWidth * multiplier, line.endWidth * multiplier);
		}
	}

	public PlayerConroller GetRandomEnemy()
	{
		int val = rng.Next(enemies.Count);
		return enemies[val];
	}

	public float GetBodyScale()
	{
		return rbBody.transform.localScale.x;
	}

	private void HandleRayCast()
	{
		Vector2 startPos = rbBody.transform.position;
		var dir = Vector2.down;
		RaycastHit2D hit = Physics2D.Raycast(startPos, dir, floorHitDistance, groundLayerMask);
		Debug.DrawRay(startPos, dir * floorHitDistance, Color.red);
		if(hit.collider != null)
		{
			dir = hit.point - new Vector2(transform.position.x, transform.position.y);
			hitDir = dir.normalized;
			isGrounded = true;
			crashForce.Crash(hit.point);
		}
		else
		{
			isGrounded = false;
		}		
	}

	public float crashBonus = 10.0f;

	private void FixedUpdate()
	{
		rbBody.velocity = Vector2.ClampMagnitude(rbBody.velocity, maxVelocity);
	}
	private void HandleMovement()
	{
		float horizontal = Input.GetAxis(inputHorizontal);
		float vertical = Input.GetAxis(inputVertical);

		currentHorizontalForce = horizontal * Time.deltaTime * horizontalForce;
		currentVerticalForce = vertical * Time.deltaTime * verticalForce;

		if(!isGrounded && currentVerticalForce > 0)
		{
			currentVerticalForce = 0;
		}
		if(currentVerticalForce < 0 && !isGrounded)
		{
			crashing = true;
			crashBonus += 5 * Time.deltaTime;
			currentVerticalForce *= crashMultiplier;
			AddForces(rbBody, ForceMode2D.Force);
		}
		else
		{
			AddForces(rbBody, ForceMode2D.Impulse);
		}
	}

	private void AddForces(Rigidbody2D rb, ForceMode2D mode = ForceMode2D.Force)
	{
		float multiplier = 1.0f;
		if(mode == ForceMode2D.Force)
		{
			multiplier *= 100;
		}
		if(!crashing)
		{
			currentHorizontalForce *= 2;
		}
		rb.AddForce(currentHorizontalForce * multiplier * Vector3.right, ForceMode2D.Force);
		rb.AddForce(currentVerticalForce * Vector3.up, mode);
	}
}
